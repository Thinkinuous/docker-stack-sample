using System;
using EasyNetQ;
using Messages;
using Microsoft.Extensions.Configuration;

namespace ReceiverConsole
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            string rabbitHost = Configuration["RabbitMQ.Host"];

            using (var bus = RabbitHutch.CreateBus(rabbitHost))
            {
                bus.Subscribe<HappyMessage>("happy-handler", HandleHappyMessage);
                bus.Subscribe<SadMessage>("sad-handler", HandleSadMessage);

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }

        private static void HandleHappyMessage(HappyMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Good: {message.ItIsGoodTo}");
            Console.ResetColor();
        }

        private static void HandleSadMessage(SadMessage message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Bad: {message.ItIsBadTo}");
            Console.ResetColor();
        }
    }
}
