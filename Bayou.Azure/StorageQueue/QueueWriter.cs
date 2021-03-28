using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Bayou.Common.Helpers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bayou.Azure.StorageQueue
{
    public class QueueWriter
    {
        private readonly QueueClient _queueClient;

        public QueueWriter(string connectionString, string queueName)
        {
            ParmCheck.NotNullOrEmpty(nameof(connectionString), connectionString);
            ParmCheck.NotNullOrEmpty(nameof(queueName), queueName);
            
            _queueClient = new QueueClient(connectionString, queueName);

            if (_queueClient.Exists() == false)
            {
                _queueClient.Create();
            }
        }

        public async Task<string> Write<TType>(TType observation)
        {
            string messageText = JsonSerializer.Serialize(observation);

            Response<SendReceipt> sendReceipt = await _queueClient.SendMessageAsync(messageText);

            return sendReceipt.Value.MessageId;
        }
    }
}
