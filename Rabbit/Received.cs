using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rabbit
{
    public class Received
    {
        private string _hostName = "localhost";
        private string _userName = "sabir";
        private string _password = "service";
        private bool _run = false;
        private Thread thread;

        public void Read(string queues, Func<string, bool> delegat)
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

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                bool er = delegat.Invoke(message);
                if (!_run || !er)
                {
                    channel.Close();
                    connection.Close();
                    thread.Abort();
                }
            };
            channel.BasicConsume(queue: queues, autoAck: true, consumer: consumer);
        }

        public void Start(string queues, Func<string, bool> delegat)
        {
            _run = true;
            thread = new Thread(() => Read(queues, delegat));
            thread.Start();
        }

        public void Stop(string queues)
        {
            _run = false;
        }
    }
}
