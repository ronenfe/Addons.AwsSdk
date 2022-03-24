using AwsSdk;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwsSdkTest
{
    [TestClass]
    public class IntegrationTestCloudFormationCient
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _regionName;
        private readonly string _serviceExportName;

        public IntegrationTestCloudFormationCient()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            _accessKey = config["access_key"];
            _secretKey = config["secret_key"];
            _regionName = config["region_name"];
            _serviceExportName = config["service_export_name"];
        }
        [TestMethod]
        public async Task ApiGatewayClientIntegrationTest()
        {
            var client = new CloudFormationClient(_accessKey, _secretKey, _regionName);
            var response = await client.GetExportValueByExportNameAsync(_serviceExportName).ConfigureAwait(false);
            Assert.IsNotNull(response);
            Assert.AreNotEqual("", response);
        }
    }
}
