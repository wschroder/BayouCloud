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
            IConfigurationRoot config = SetupConfiguration();

            string connectionString = config["StorageServiceConnectionString"];

            var queueTester = new StorageQueueTester(connectionString);

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
