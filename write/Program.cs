using Rabbit;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace write
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string mes;
            bool close = true;
            Sending sending = new Sending("localhost", "sabir", "service");
            while (close)
            {
                Console.WriteLine("Введите сообщение");
                mes = Console.ReadLine();
                sending.Send(mes, "test2");
                Console.WriteLine("для выхода нажмите интер");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    close = false;
                }
            }
        }
    }
}
