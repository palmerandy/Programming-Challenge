using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Products.ApiIntegrationTests
{
    public class BaseTest
    {
        private const string AuthorizationHeaderName = "Authorization";

        protected HttpClient Client;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44356/"),
                Timeout = TimeSpan.FromSeconds(2)
            };
            Client.DefaultRequestHeaders.Add("Accept", "application/json");
            AddApiKeyAuthorization();
        }

        protected void AddApiKeyAuthorization()
        {
            Client.DefaultRequestHeaders.Add(AuthorizationHeaderName, "ApiKey sample-key");
        }
        protected void RemoveApiKeyAuthorization()
        {
            Client.DefaultRequestHeaders.Remove(AuthorizationHeaderName);
        }

        protected StringContent BuildStringContent(object body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }

        protected async Task<T> ReadContent<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}