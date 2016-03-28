using System;
using EasyNetQ;
using Messages;
using Microsoft.Extensions.Configuration;

namespace SenderConsole
{
    public class Program
    {
        static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            string rabbitHost = Configuration["RabbitMQ.Host"];

            using (var bus = RabbitHutch.CreateBus(rabbitHost))
            {
                var input = "";
                Console.WriteLine("Enter a happy message and we'll handle it! 'q' to quit.");
                while ((input = Console.ReadLine()) != "q")
                {
                    bus.Publish(new HappyMessage {
                        ItIsGoodTo = $"It is good to {input}"
                    });

                    bus.Publish(new SadMessage {
                        ItIsBadTo = $"it is bad not to {input}"
                    });
                }
            }
        }
    }
}
