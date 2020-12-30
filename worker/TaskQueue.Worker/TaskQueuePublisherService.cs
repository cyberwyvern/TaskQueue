using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace TaskQueueWorker
{
    public class TaskQueuePublisherService : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public TaskQueuePublisherService(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory { HostName = _configuration["QueueConnection"] };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration["QueueOut"],
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void Push(TaskEntity taskEntity)
        {
            _channel.BasicPublish(exchange: "",
                                  routingKey: _configuration["QueueOut"],
                                  basicProperties: null,
                                  body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(taskEntity)));
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}