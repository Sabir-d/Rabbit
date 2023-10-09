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
        private string _hostName;
        private string _userName;
        private string _password;
        private bool _run = false;
        private bool conect = false;
        private bool er = false;
        private Thread thread;
        IConnection connection;

        public Received(string hostName, string userName, string password)
        {
            _hostName = hostName;
            _userName = userName;
            _password = password;
        }

        public void Read(string queues, Func<string, bool> delegat)
        {
            while (_run && !er)
            {
                try
                {
                    if (!conect)
                    {
                        Console.WriteLine("Connect....");
                        var factory = new ConnectionFactory
                        {
                            HostName = _hostName,
                            UserName = _userName,
                            Password = _password
                        };
                        connection = factory.CreateConnection();
                        Console.WriteLine("Connect: Ok");
                        var channel = connection.CreateModel();

                        channel.QueueDeclare(
                            queue: queues,
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null
                        );
                        Console.WriteLine("channel: Ok");
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
                    conect = connection.IsOpen;
                }
                catch
                {
                    Thread.Sleep(1000);
                    conect = false;
                }
            }
        }

        public void Start(string queues, Func<string, bool> delegat)
        {
            _run = true;
            thread = new Thread(() => Read(queues, delegat));
            thread.Start();
        }

        public void Stop()
        {
            _run = false;
        }
    }
}
