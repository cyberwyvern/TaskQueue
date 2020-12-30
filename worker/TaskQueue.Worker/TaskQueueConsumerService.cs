using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskQueueWorker
{
    public class TaskQueueConsumerService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly TaskQueuePublisherService _callbackQueue;
        private readonly Random _rand;

        public TaskQueueConsumerService(IConfiguration configuration, TaskQueuePublisherService callbackQueue)
        {
            _configuration = configuration;
            _callbackQueue = callbackQueue;
            _rand = new Random();

            var factory = new ConnectionFactory
            {
                HostName = _configuration["QueueConnection"],
                DispatchConsumersAsync = true
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclarePassive(_configuration["QueueIn"]);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var taskEntity = JsonConvert.DeserializeObject<TaskEntity>(Encoding.UTF8.GetString(ea.Body.Span));

                try
                {
                    taskEntity.StartedDate = DateTime.UtcNow;
                    await DoWorkAsync(taskEntity, cancellationToken);
                    taskEntity.CompletedDate = DateTime.UtcNow;
                    taskEntity.Status = TaskStatus.Completed;
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch
                {
                    taskEntity.CompletedDate = DateTime.UtcNow;
                    taskEntity.Status = TaskStatus.Failed;
                    _channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                }

                _callbackQueue.Push(taskEntity);
            };

            _channel.BasicConsume(queue: _configuration["QueueIn"], autoAck: false, consumer: consumer);

            return Task.CompletedTask;
        }

        public async Task DoWorkAsync(TaskEntity taskEntity, CancellationToken cancellationToken)
        {
            //emulate processing
            await Task.Delay(_rand.Next(2, 16) * 1000, cancellationToken);
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