using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Models.Shop;
using Shop.Application.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShopTests
{
    public class ShopApiTests
    {
        private HttpClient client;

        public ShopApiTests()
        {
            var projectDir = Directory.GetCurrentDirectory();
            string testsDirectoryPath = Path.GetFullPath(Path.Combine(projectDir, @"..\..\..\"));
            var configuration = (new ConfigurationBuilder().SetBasePath(testsDirectoryPath).AddJsonFile("appsettings.json")).Build();
            var builder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseEnvironment("Development")
                .UseStartup<ShopApiTestsStartup>();
            TestServer testServer = new TestServer(builder);
            client = testServer.CreateClient();
        }

        [Fact]
        public async Task CheckFailedAuthorization()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "test" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("https://localhost:44311/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CheckSuccessAuthorization()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "Enigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("https://localhost:44311/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(result.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);
        }

        [Fact]
        public async Task CheckGetWithoutAuthorization()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/shop/5/article"));
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckBuyWithoutAuthorization()
        {
            var article = new ArticleDto { ID = 3, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44311/api/shop/buy";
            var content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task CheckGetArticleFromCacheSuccess()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "Enigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44311/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/shop/3/article"));
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var article = JsonConvert.DeserializeObject<ArticleDto>(response.Content.ReadAsStringAsync().Result);
            Assert.NotNull(article);
            Assert.Equal(3, article.ID);
        }

        [Fact]
        public async Task CheckGetArticleFromWarehouseSuccess()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "Enigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44311/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44311/api/shop/13/article"));
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var article = JsonConvert.DeserializeObject<ArticleDto>(response.Content.ReadAsStringAsync().Result);
            Assert.NotNull(article);
            Assert.Equal(13, article.ID);
        }

        [Fact]
        public async Task CheckBuyArticleFromWarehouseSuccess()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "Enigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44311/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            var article = new ArticleDto { ID = 13, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44311/api/shop/buy";
            content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");
            ;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.PostAsync(url, content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var bought = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
            Assert.True(bought);
        }
    }
}
