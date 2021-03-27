using System.Threading.Tasks;

namespace Bayou.Cloud.IntegrationTest
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=wlsstorage779;AccountKey=DvygnXM/eD8G03N9WLwNEI4BpnioyUveK/qzKovLf0zl5aKInokZ0FoLiq+NhlDpjj3d5fx4ynOSP5aKQz8KYg==;EndpointSuffix=core.windows.net";

            var queueTester = new StorageQueueTester(connectionString);

            await queueTester.RunTest();
        }
    }
}
