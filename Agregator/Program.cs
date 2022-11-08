using System;
using System.Threading;

namespace Agregator
{
    class Program
    {
        private static bool _keepRunning = true;

        static void Main(string[] args)
        {
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                Program._keepRunning = false;
            };

            Console.WriteLine("Starting HTTP listener...");

            var httpServer = new Server();
            Threads threads = new Threads();
            foreach (var var in threads.ExtractThreads())
            {
                var.Start();
            }

            foreach (var var in threads.ReceiverThreads())
            {
                var.Start();
            }

            httpServer.Start();
            while (Program._keepRunning)
            {
            }

            httpServer.Stop();

            Console.WriteLine("Exiting gracefully...");
        }
    }
}