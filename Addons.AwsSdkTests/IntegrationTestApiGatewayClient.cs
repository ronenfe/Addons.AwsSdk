using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwsSdkAddons;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AwsSdkTest
{
    [TestClass]
    public class ApiGatewayClientTest
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _regionName;
        private readonly string _serviceRoute;
        private readonly string _serviceExportName;
        private readonly string _json;

        public ApiGatewayClientTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _accessKey = config["access_key"];
            _secretKey = config["secret_key"];
            _regionName = config["region_name"];
            _serviceRoute = config["service_route"];
            _serviceExportName = config["service_export_name"];
            _json = config["json"];
        }
        [TestMethod]
        public async Task ApiGatewayClientIntegrationTest()
        {
            var client = new ApiGatewayClient(_accessKey, _secretKey, _regionName, _serviceExportName, _serviceRoute);

            var response = await client.PostAsync(_json).ConfigureAwait(false);
            Assert.IsNotNull(response);
            Assert.AreNotEqual("", response);
        }
    }
}
