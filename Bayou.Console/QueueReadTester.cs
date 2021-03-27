using Bayou.Azure.StorageQueue;
using Bayou.Domain.Fishery;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bayou.Console
{
    public class QueueReadTester
    {
        public async Task Write()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=wlsstorage779;AccountKey=DvygnXM/eD8G03N9WLwNEI4BpnioyUveK/qzKovLf0zl5aKInokZ0FoLiq+NhlDpjj3d5fx4ynOSP5aKQz8KYg==;EndpointSuffix=core.windows.net";
            string queueName = "fish-sightings";

            var reader = new QueueReader(connectionString, queueName);

            
            while (true)
            {
                FishObservation observation = await reader.Read<FishObservation>();

                if (observation == null)
                {
                    System.Console.WriteLine("\r\nNo more data available.  Hit any key to exit...");
                    System.Console.ReadKey();
                    break;
                }

                var options = new JsonSerializerOptions { WriteIndented = true };

                System.Console.WriteLine(JsonSerializer.Serialize(observation, typeof(FishObservation), options));

                System.Console.WriteLine("\r\nHit 'q' to exit, or any other key to continue...");
                ConsoleKeyInfo key = System.Console.ReadKey();
                if (key.KeyChar == 'q')
                {
                    break;
                }
            }
        }

    }
}
