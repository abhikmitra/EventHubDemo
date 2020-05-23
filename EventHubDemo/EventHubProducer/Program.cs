using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;

namespace EventHubProducer
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string EventHubConnectionString = "{Event Hubs connection string}";
        private const string EventHubName = "{Event Hub path/name}";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(100);


            while (true)
            {
                await Task.Delay(100);
            }

            await eventHubClient.CloseAsync();
        }

        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var message = $"Message {i}";
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}
