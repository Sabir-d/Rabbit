using System;
using Rabbit;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace read
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Received received = new Received("localhost","sabir","service");

            received.Start("test2", test);
            Console.WriteLine("для остановки нажмите клавишу");
            Console.ReadKey();
            received.Stop();
            Console.WriteLine("для выхода нажмите клавишу");
            Console.ReadKey();
            bool test(string a)
            {
                Console.WriteLine(a);
                return true;
            }
        }
    }
}
