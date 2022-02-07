using System.Net.Http;
using System.Threading.Tasks;
using ECommerceBackend.Core.Entities.ProductEntities;
using ECommerceBackend.Web;
using Newtonsoft.Json;
using Xunit;

namespace ECommerceBackend.FunctionalTests.Api.OrganizationEndpoints
{
    public class GetById_Should : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GetById_Should(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        //[Fact]
        public async Task Include_OrganizationContact()
        {
            var response = await _client.GetAsync("Products/1");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(stringResponse);

            Assert.NotNull(result);
        }
    }
}
