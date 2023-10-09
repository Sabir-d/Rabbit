using System;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;

namespace Rabbit
{
    public class Sending
    {
        private string _hostName;
        private string _userName;
        private string _password;
        private bool conect = false;
        public Sending(string hostName, string userName, string password)
        {
            _hostName = hostName;
            _userName = userName;
            _password = password;
        }

        public void Send(string message, string queues)
        {
            while (!conect)
            {
                try
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

                    conect = connection.IsOpen;
                }
                catch { 
                
                }
                } 
        }
    }
}
