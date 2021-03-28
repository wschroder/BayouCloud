using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Bayou.Common.Helpers;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bayou.Azure.StorageQueue
{
    public class QueueReader
    {
        private readonly QueueClient _queueClient;

        public QueueReader(string connectionString, string queueName)
        {
            ParmCheck.NotNullOrEmpty(nameof(connectionString), connectionString);
            ParmCheck.NotNullOrEmpty(nameof(queueName), queueName);

            _queueClient = new QueueClient(connectionString, queueName);
            if (_queueClient.Exists() == false)
            {
                throw new InvalidOperationException($"Queue not found: {queueName}");
            }
        }

        public async Task<TReadType> Read<TReadType>() where TReadType:class
        {
            Response<QueueMessage> response = await _queueClient.ReceiveMessageAsync();

            if (response.Value?.MessageId == null)
            {
                return null;
            }
            TReadType result = JsonSerializer.Deserialize<TReadType>(response.Value.Body);

            await _queueClient.DeleteMessageAsync(response.Value.MessageId, response.Value.PopReceipt);

            return result;
        }
    }
}
