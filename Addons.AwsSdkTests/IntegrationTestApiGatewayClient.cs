using System;
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
        private readonly string _json;
        private readonly string _address;

        public ApiGatewayClientTest()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _accessKey = config["access_key"];
            _secretKey = config["secret_key"];
            _regionName = config["region_name"];
            _address = config["address"];
            _json = config["json"];
        }
        [TestMethod]
        public async Task ApiGatewayClientIntegrationTest()
        {
            var client = new ApiGatewayClient(_accessKey, _secretKey, _regionName);

            var response = await client.PostAsync(new Uri(_address), _json).ConfigureAwait(false);
            Assert.IsNotNull(response);
            Assert.AreNotEqual("", response);
        }
    }
}
