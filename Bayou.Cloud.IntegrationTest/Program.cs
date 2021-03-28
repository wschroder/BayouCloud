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
            // string connectionString = "DefaultEndpointsProtocol=https;AccountName=wlsstorage779;AccountKey=DvygnXM/eD8G03N9WLwNEI4BpnioyUveK/qzKovLf0zl5aKInokZ0FoLiq+NhlDpjj3d5fx4ynOSP5aKQz8KYg==;EndpointSuffix=core.windows.net";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            if (IsDevelopment())
            {
                configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly());
            }
            IConfigurationRoot config = configBuilder.Build();
            string connectionString = config["StorageServiceConnectionString"];

            var queueTester = new StorageQueueTester(connectionString);

            await queueTester.RunTest();
        }

        private static bool IsDevelopment()
        {
            string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            bool isDev = (string.IsNullOrEmpty(envName) || envName.Equals("development", StringComparison.OrdinalIgnoreCase));
            return isDev;
        }
    }
}
