using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Bayou.Cloud.IntegrationTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IConfigurationRoot config = SetupConfiguration();

            string keyVaultName = config["KeyVaultName"];

            config = MergeInKeyVaultConfig(config, keyVaultName);

            string storageServiceConnectionString = config["StorageServiceConnectionString"];

            var queueTester = new StorageQueueTester(storageServiceConnectionString);

            await queueTester.RunTest();
        
        }

        private static IConfigurationRoot MergeInKeyVaultConfig(IConfigurationRoot config, string keyVaultName)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder();

            configBuilder.AddConfiguration(config);

            List<KeyValuePair<string, string>> secretsList = DownloadKeyVaultValues(keyVaultName);

            configBuilder.AddInMemoryCollection(secretsList);

            return configBuilder.Build();
        }

        private static List<KeyValuePair<string, string>> DownloadKeyVaultValues(string keyVaultName)
        {
            // Note: Access to the keyvault relies on Role Based Access Control (RBAC), 
            // with this console application's identify configured as environment variables
            // (AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, AZURE_TENANT_ID) that are picked up by
            // DefaultAzureCredentials.
            // Sett Setup-KeyVault.ps1 for details on setting this up.

            var kvUri = $"https://{keyVaultName}.vault.azure.net";

            var cred = new DefaultAzureCredential();
            // var cred = new VisualStudioCredential();

            var client = new SecretClient(new Uri(kvUri), cred);

            List<KeyValuePair<string, string>> secretsList = new List<KeyValuePair<string, string>>();

            IEnumerable<SecretProperties> secretProperties = client.GetPropertiesOfSecrets();
            foreach (SecretProperties secret in secretProperties)
            {
                KeyVaultSecret secretWithValue = client.GetSecret(secret.Name);
                var nameValuePair = new KeyValuePair<string, string>(secretWithValue.Name, secretWithValue.Value);
                secretsList.Add(nameValuePair);
            }
            return secretsList;
        }

        private static IConfigurationRoot SetupConfiguration()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            if (IsDevelopment())
            {
                configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            }
            configBuilder.AddJsonFile("AppSettings.json");
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
