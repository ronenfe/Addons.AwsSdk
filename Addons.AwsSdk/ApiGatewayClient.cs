using Aws4RequestSigner;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwsSdkAddons
{
    public class ApiGatewayClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly string _regionName;
        private readonly string _secretKey;
        private readonly string _accessKey;

        public ApiGatewayClient(string accessKey, string secretKey, string regionName)
        {
            _secretKey = secretKey;
            _accessKey = accessKey;
            _regionName = regionName;       
        }
        private async Task SignAsync(HttpRequestMessage request)
        {
            var signer = new AWS4RequestSigner(_accessKey, _secretKey);
            await signer.Sign(request, "execute-api", _regionName).ConfigureAwait(false);
        }
        public async Task<string> PostAsync(Uri address, string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
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
