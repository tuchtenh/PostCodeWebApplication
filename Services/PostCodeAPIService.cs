using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PostCodeWebApplication.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostCodeWebApplication.Services
{
    public class PostCodeAPIService : IPostCodeService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public PostCodeAPIService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config.GetValue<string>("WebAPIKey");
        }

        public async Task<string> GetPostCode(string address)
        {
            var url = String.Format("/?address={0}&key={1}",address,_apiKey);
            var response = await _client.GetAsync(url);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PostItResult>(apiResponse);

            return result.data.FirstOrDefault()?.post_code;

        }
    }
}
