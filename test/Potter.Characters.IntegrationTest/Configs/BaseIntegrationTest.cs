using Microsoft.AspNetCore.Mvc.Testing;
using Potter.Characters.Api;
using System.Net.Http;

namespace Potter.Characters.IntegrationTest.Configs
{
    public class BaseIntegrationTest
    {
        protected readonly HttpClient _httpClient;

        public BaseIntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            _httpClient = appFactory.CreateClient();
        }
    }
}
