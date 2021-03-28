using Bayou.Azure.StorageQueue;
using Bayou.Common.Exceptions;
using Bayou.Common.Helpers;
using Bayou.Domain.Fishery;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bayou.Cloud.IntegrationTest
{
    public class StorageQueueTester
    {
        private readonly string _connectionString;

        public StorageQueueTester(string connectionString)
        {
            ParmCheck.NotNullOrEmpty(nameof(connectionString), connectionString);

            _connectionString = connectionString;
        }

        public async Task RunTest()
        {
            const string queueName = "fish-sightings";
            const int numWrites = 5;

            await PerformWrites(queueName, numWrites);

            int numReads = await PerformReads(queueName);

            if (numReads != numWrites)
            {
                throw new TestException($"Wrote {numWrites} to queue {queueName} but only read {numReads} messages back.");
            }
            Console.WriteLine($"{numWrites} messages written.");
            Console.WriteLine($"{numReads} messages read.");
        }

        private async Task PerformWrites(string queueName, int numWrites)
        {
            ParmCheck.NotNullOrEmpty(nameof(queueName), queueName);
            ParmCheck.Positive(nameof(numWrites), numWrites);

            var queueWriter = new QueueWriter(_connectionString, queueName);

            for (int i = 0; i < numWrites; i++)
            {
                FishObservation observation = DataGenerator.CreateFishObservation();

                string messageId = await queueWriter.Write(observation);

                Console.WriteLine($"Message {messageId} sent.");
            }
        }

        private async Task<int> PerformReads(string queueName)
        {
            ParmCheck.NotNullOrEmpty(nameof(queueName), queueName);

            int numRead = 0;
            var options = new JsonSerializerOptions { WriteIndented = true };

            var reader = new QueueReader(_connectionString, queueName);

            FishObservation observation = await reader.Read<FishObservation>();

            while (observation != null)
            {
                numRead++;
                Console.WriteLine(JsonSerializer.Serialize(observation, typeof(FishObservation), options));
                observation = await reader.Read<FishObservation>();
            }
            return numRead;
        }
    }
}
