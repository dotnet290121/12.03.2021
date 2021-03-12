using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueuePatternFriday
{
    class Program
    {
        static int counter = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            string pwd = "1234";
            string pwd_user = "";

            do
            {
                Console.WriteLine("Enter password:");
                pwd_user = Console.ReadLine();
            } while (pwd_user != pwd);

            Console.WriteLine("You get prize!");


            Stopwatch sw = new Stopwatch();
            sw.Start();
            WorkerQueue queue = new WorkerQueue(10);
            for (int i = 0; i < 150000; i++)
            {
                int j = i;
                queue.Produce(() => { Console.WriteLine($"new work {j}"); Thread.Sleep(1);
                    Console.WriteLine("Completed...");
                    counter++;
                });
            }

            while (counter < 12)
            {
                Thread.Sleep(10);
            }
            sw.Stop();
            Console.WriteLine($"Took {sw.Elapsed}");


            Console.WriteLine("main completed...");
        }
    }
}
