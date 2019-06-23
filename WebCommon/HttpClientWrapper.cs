using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebCommon
{
    /// <summary>
    /// https://tesla-api.timdorr.com/
    /// </summary>
    public class HttpClientWrapper
    {
        public HttpClient Client { get; set; }
        public HttpClientWrapper(string userAgent, string baseUrl)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            Client.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(userAgent, "1"));
        }

        public void SetAuthorizationHeader(string accessToken)
        {
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<WebRequestResult<TResponse>> SendAsync<TResponse>(HttpMethod method, string url)
        {
            return await _processRequest<TResponse>(new HttpRequestMessage(method, url));
        }

        public async Task<WebRequestResult<TResponse>> SendAsync<TResponse, TRequest>(HttpMethod method, string url, TRequest dto, string contentType = "application/json")
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                if (dto != null)
                    request.Content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, contentType);

                return await _processRequest<TResponse>(request);
            }
            catch (Exception e)
            {
                return new WebRequestResult<TResponse>
                {
                    Exception = e
                };
            }
        }

        private async Task<WebRequestResult<TResponse>> _processRequest<TResponse>(HttpRequestMessage request)
        {
            try
            {
                var serviceResponse = await Client.SendAsync(request);

                if (serviceResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new WebRequestResult<TResponse>
                    {
                        ServiceResponse = serviceResponse
                    };
                }

                var deserializedResult = JsonConvert.DeserializeObject<TResponse>(await serviceResponse.Content.ReadAsStringAsync());

                return new WebRequestResult<TResponse>
                {
                    Result = deserializedResult,
                    ServiceResponse = serviceResponse
                };
            }
            catch (Exception e)
            {
                return new WebRequestResult<TResponse>
                {
                    Exception = e
                };
            }
        }
    }
}
