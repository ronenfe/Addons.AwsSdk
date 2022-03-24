using Aws4RequestSigner;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwsSdk
{
    public class ApiGatewayClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly string _regionName;
        private readonly string _secretKey;
        private readonly string _accessKey;
        private readonly string _serviceExportName;
        private readonly string _ServiceRoute;

        public ApiGatewayClient(string accessKey, string secretKey, string regionName, string serviceExportName, string serviceRoute)
        {
            _secretKey = secretKey;
            _accessKey = accessKey;
            _regionName = regionName;
            _serviceExportName = serviceExportName;
            _ServiceRoute = serviceRoute;
            
        }
       
        private async Task SignAsync(HttpRequestMessage request)
        {
            var signer = new AWS4RequestSigner(_accessKey, _secretKey);
            request = await signer.Sign(request, "execute-api", _regionName).ConfigureAwait(false);
        }
        public async Task<Uri> CreateServiceUrlAsync()
        {
            var cloudFormationClient = new CloudFormationClient(_accessKey, _secretKey, _regionName);
            var serviceExportValue = await cloudFormationClient.GetExportValueByExportNameAsync(_serviceExportName).ConfigureAwait(false);
            return new Uri(serviceExportValue + _ServiceRoute);
        }
        public async Task<string> PostAsync(string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var address = await CreateServiceUrlAsync().ConfigureAwait(false);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = address,
                Content = content,
            };
            await SignAsync(request).ConfigureAwait(false);
            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);

            if (response == null)
            {
                throw new Exception("AWS response is null.");
            }
            else if (response.Content == null)
            {
                throw new Exception("AWS response.Content is null.");
            }
            else
            {
                var ResponseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(ResponseContent);
                }
                else
                {
                    return ResponseContent;
                }
            }
        }
        
    }

}
