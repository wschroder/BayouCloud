using Bayou.Azure.KeyVault;
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

            List<KeyValuePair<string, string>> secretsList = KeyVaultManager.DownloadKeyVaultValues(keyVaultName);

            configBuilder.AddInMemoryCollection(secretsList);

            return configBuilder.Build();
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
