using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskQueue.DAO.Interface;
using TaskQueue.API.DTO;
using Microsoft.AspNetCore.SignalR;
using TaskQueue.API.Hubs;
using AutoMapper;
using TaskQueue.API.Models;
using TaskQueue.DAO.Entities;

namespace TaskQueue.API.Services
{
    public class TaskQueueConsumerSevice : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly IHubContext<TaskHub, ITaskClient> _taskHub;

        public TaskQueueConsumerSevice(
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory,
            IMapper mapper,
            IHubContext<TaskHub, ITaskClient> taskHub)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _taskHub = taskHub;

            ConnectionFactory factory = new ConnectionFactory {
                HostName = _configuration["QueueConnection"],
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration["QueueIn"],
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                TaskEntityDTO item = JsonConvert.DeserializeObject<TaskEntityDTO>(Encoding.UTF8.GetString(ea.Body.Span));

                try
                {
                    var taskEntity = DoWork(item);
                    await NotifyClientAsync(taskEntity);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch
                {
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: _configuration["QueueIn"], autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public TaskEntity DoWork(TaskEntityDTO taskEntityDTO)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IDatabaseUnitOfWork db = scope.ServiceProvider.GetRequiredService<IDatabaseUnitOfWork>();
            try
            {
                var taskEntity = db.Tasks.Read(taskEntityDTO.Id);
                taskEntity.StartedDate = taskEntityDTO.StartedDate;
                taskEntity.CompletedDate = taskEntityDTO.CompletedDate;
                taskEntity.Status = taskEntityDTO.Status;

                db.StartTransaction();
                db.Tasks.Update(taskEntity);
                db.Commit();

                return taskEntity;
            }
            catch
            {
                db.Rollback();
                throw;
            }
        }

        private async Task NotifyClientAsync(TaskEntity taskEntity)
        {
            var group = taskEntity.UserProfile.UserId.ToString();
            var taskModel = _mapper.Map<TaskModel>(taskEntity);
            await _taskHub.Clients.Group(group).TaskChanged(taskModel);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}