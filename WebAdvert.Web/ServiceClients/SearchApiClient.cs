using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebAdvert.Web.ServiceClients
{
    public class SearchApiClient : ISearchApiClient
    {
        private readonly HttpClient _client;
        private readonly string _baseAddress = string.Empty;

        public SearchApiClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _baseAddress = configuration.GetSection("SearchApi").GetValue<string>("url");
        }

        public async Task<List<AdvertType>> Search(string keyword)
        {
            var result = new List<AdvertType>();

            var callUrl = $"{_baseAddress}/search/v1/{keyword}";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, callUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = await _client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var allAdverts = await response.Content.ReadAsAsync<List<AdvertType>>().ConfigureAwait(false);
                    result.AddRange(allAdverts);
                }

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}