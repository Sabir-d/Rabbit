using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rabbit
{
    public class Sending
    {
        private string _hostName = "192.168.1.83";
        private string _userName = "sabir";
        private string _password = "service";

        public void Send(string message, string queues)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: queues,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queues,
                basicProperties: null,
                body: body
            );
        }
    }
}
