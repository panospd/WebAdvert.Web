using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AdvertApi.Models;
using Amazon.ServiceDiscovery;
using Amazon.ServiceDiscovery.Model;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebAdvert.Web.Services;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _configuration = configuration;
            _client = client;
            _mapper = mapper;

            var discoveryClient = new AmazonServiceDiscoveryClient();

            var discoveryTask = discoveryClient.DiscoverInstancesAsync(new DiscoverInstancesRequest
            {
                ServiceName = "advertapi",
                NamespaceName = "WEbAdvertisement"
            });

            discoveryTask.Wait();

            var instances = discoveryTask.Result.Instances;

            var ipv4 = instances[0].Attributes["AWS_INSTANCE_IPV4"];
            var port = instances[0].Attributes["AWS_INSTANCE_PORT"];
            
            var baseUrl = _configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
            _client.BaseAddress = new Uri(baseUrl);
        }

        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            var advertApiModel = _mapper.Map<AdvertModel>(model);

            var jsonModel = JsonConvert.SerializeObject(advertApiModel);
            var address = $"{_client.BaseAddress}/create";
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, address);
            request.Content = new StringContent(jsonModel);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.SendAsync(request);

            var responseJson = await response.Content.ReadAsStringAsync();
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);

            return _mapper.Map<AdvertResponse>(createAdvertResponse);
        }

        public async Task<bool> Confirm(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(advertModel);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{_client.BaseAddress}/confirm");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(jsonModel);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Advertisement>> GetAllAsync()
        {
            string callUri = $"{_client.BaseAddress}/all";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, callUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var apiCallResponse = await _client.SendAsync(request);

                var allAdvertModels = await apiCallResponse.Content.ReadAsAsync<List<AdvertModel>>();

                return allAdvertModels.Select(x => _mapper.Map<Advertisement>(x)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            
        }
    }
}
