﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shared.Models.Models;
using Shared.Models.Shop;
using Shop.Application.Core.Interfaces;
using Shop.Application.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Core.Integration
{
    public class DealerCommunicator : IDealerCommunicator
    {
        private readonly AppSettings _appSettings;
        private readonly IMemoryCache _cache;

        public DealerCommunicator(IOptions<AppSettings> appSettings, IMemoryCache memoryCache)
        {
            _appSettings = appSettings.Value;
            _cache = memoryCache;
        }

        public async Task<ArticleDto> GetArticle(int id, HttpCommunicatorEnum httpDealer)
        {
            var httpClientURL = httpDealer == HttpCommunicatorEnum.DealerOne ? _appSettings.DealerOneURL : _appSettings.DealerTwoURL;
            using (var client = new HttpClient())
            {
                string bearerToken = await GetVendorAuthorizationToken(client);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"{httpClientURL}/{id}/article/"));
                if(response.IsSuccessStatusCode)
                {
                    var article = JsonConvert.DeserializeObject<ArticleDto>(response.Content.ReadAsStringAsync().Result);
                    return article;
                }

                return null;
            }
        }

        public async Task<bool> BuyArticle(ArticleDto article, HttpCommunicatorEnum httpDealer)
        {
            var httpClientURL = httpDealer == HttpCommunicatorEnum.DealerOne ? _appSettings.DealerOneURL : _appSettings.DealerTwoURL;
            using (var client = new HttpClient())
            {
                string serializedArticle = JsonConvert.SerializeObject(article);
                string url = $"{httpClientURL}/buy";
                var content = new StringContent(serializedArticle, Encoding.UTF8, "application/json");

                string bearerToken = await GetVendorAuthorizationToken(client);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                var result = await client.PostAsync(url, content);
                if (result.IsSuccessStatusCode)
                {
                    var articleBought = JsonConvert.DeserializeObject<bool>(result.Content.ReadAsStringAsync().Result);
                    return articleBought;
                }

                return false;
            }
        }

        private async Task<string> GetVendorAuthorizationToken(HttpClient client)
        {
            string bearerToken;
            if (!_cache.TryGetValue("token", out bearerToken))
            {
                GetTokenDto getTokenDto = new GetTokenDto { Passcode = _appSettings.VendorAuthPasscode };
                string serializedAuthTokenBody = JsonConvert.SerializeObject(getTokenDto);
                var content = new StringContent(serializedAuthTokenBody, Encoding.UTF8, "application/json");

                var result = await client.PostAsync(_appSettings.VendorAuthURL, content);
                var token = JsonConvert.DeserializeObject<TokenDto>(result.Content.ReadAsStringAsync().Result);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(120));

                _cache.Set("token", token.Token, cacheEntryOptions);
                bearerToken = token.Token;
            }

            return bearerToken;
        }
    }
}
