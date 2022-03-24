using Amazon;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwsSdkAddons
{
    public class CloudFormationClient
    {
        private readonly string _regionName;
        private readonly string _secretKey;
        private readonly string _accessKey;

        public CloudFormationClient( string accessKey, string secretKey, string regionName)
        {
            _secretKey = secretKey;
            _accessKey = accessKey;
            _regionName = regionName;
        }
        public async Task<string> GetExportValueByExportNameAsync(string exportName)
        {
            Export serviceExportKey = null;
            List<Export> exports = null;
            string nextToken = null;
            AmazonCloudFormationClient amazonCloudFormationClient = new AmazonCloudFormationClient(_accessKey, _secretKey, RegionEndpoint.GetBySystemName(_regionName));
            do
            {
                var response = await amazonCloudFormationClient.ListExportsAsync(new ListExportsRequest { NextToken = nextToken }).ConfigureAwait(false);
                nextToken = response.NextToken;
                exports = response.Exports;
                serviceExportKey = exports?.Find(e => e.Name == exportName);
                if (serviceExportKey != null)
                    return serviceExportKey.Value;
            }
            while (nextToken != null);

            throw new Exception($"Export Name: {exportName} does not exist in Export names in region:{_regionName}");
        }
    }
}
