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
    public class VendorApiTests
    {
        private HttpClient client;

        public VendorApiTests()
        {
            var projectDir = Directory.GetCurrentDirectory();
            string testsDirectoryPath = Path.GetFullPath(Path.Combine(projectDir, @"..\..\..\"));
            var configuration = (new ConfigurationBuilder().SetBasePath(testsDirectoryPath).AddJsonFile("appsettingsvendor.json")).Build();
            var builder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseEnvironment("Development")
                .UseStartup<VendorApiTestsStartup>();
            TestServer testServer = new TestServer(builder);
            client = testServer.CreateClient();
        }

        [Fact]
        public async Task CheckFailedAuthorization()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "test" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CheckSuccessAuthorization()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var result = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, result.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(result.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);
        }

        [Fact]
        public async Task CheckGetWithoutAuthorization()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealerone/5/article"));
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckBuyWithoutAuthorization()
        {
            var article = new ArticleDto { ID = 3, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44379/api/dealerone/buy";
            var content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckDealer2GetWithoutAuthorization()
        {
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealertwo/5/article"));
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckDealer2BuyWithoutAuthorization()
        {
            var article = new ArticleDto { ID = 3, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44379/api/dealertwo/buy";
            var content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CheckGetArticleFromDealerOneSuccess()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealerone/23/article"));
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var article = JsonConvert.DeserializeObject<ArticleDto>(response.Content.ReadAsStringAsync().Result);
            Assert.NotNull(article);
            Assert.Equal(23, article.ID);
        }

        [Fact]
        public async Task CheckGetArticleFromDealerTwoSuccess()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealertwo/33/article"));
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var article = JsonConvert.DeserializeObject<ArticleDto>(response.Content.ReadAsStringAsync().Result);
            Assert.NotNull(article);
            Assert.Equal(33, article.ID);
        }

        [Fact]
        public async Task CheckGetArticleFromDealerOneFailure()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealerone/5/article"));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CheckGetArticleFromDealerTwoFailed()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44379/api/dealertwo/5/article"));
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CheckBuyArticleFromDealer1Success()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            var article = new ArticleDto { ID = 23, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44379/api/dealerone/buy";
            content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");
            ;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            response = await client.PostAsync(url, content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            var bought = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
            Assert.True(bought);
        }

        [Fact]
        public async Task CheckBuyArticleFromDealer2Success()
        {
            GetTokenDto getTokenDto = new GetTokenDto { Passcode = "VendorEnigmatry" };
            string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
            var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:44379/api/auth/GetToken", content);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var token = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            Assert.NotNull(token);
            Assert.NotNull(token.Token);

            var article = new ArticleDto { ID = 33, ArticlePrice = 122, Name_of_article = "test" };
            string serializedArticle = JsonConvert.SerializeObject(article);
            string url = $"https://localhost:44379/api/dealertwo/buy";
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
