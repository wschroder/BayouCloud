using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Bayou.Cloud.IntegrationTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // Note: Access to the keyvault relies on Role Based Access Control (RBAC), 
            // with this console application's identify configured as environment variables
            // (AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, AZURE_TENANT_ID) that are picked up by
            // DefaultAzureCredentials.

            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var cred = new DefaultAzureCredential();
            // var cred = new VisualStudioCredential();

            var client = new SecretClient(new Uri(kvUri), cred);
  
            Response<KeyVaultSecret> response = await client.GetSecretAsync(name: "StorageServiceConnectionString");

            string storageServiceConnectionString = response.Value.Value;

            // To load config from UserSecrets, use this...
            // IConfigurationRoot config = SetupConfiguration();
            // string storageServiceConnectionString = config["StorageServiceConnectionString"];

            var queueTester = new StorageQueueTester(storageServiceConnectionString);

            await queueTester.RunTest();
        
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            if (IsDevelopment())
            {
                configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            }
            IConfigurationRoot config = configBuilder.Build();
            return config;
        }

        private static bool IsDevelopment()
        {
            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isDev = (string.IsNullOrEmpty(envName) || envName.Equals("development", StringComparison.OrdinalIgnoreCase));
            return isDev;
        }
    }
}
