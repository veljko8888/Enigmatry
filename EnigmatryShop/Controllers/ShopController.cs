using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Models.Contracts;
using Shared.Models.Shop;
using Shop.Application.Core.Interfaces;

namespace Shop.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : BaseApiController
    {
        private readonly ILogger<ShopController> _log;
        private readonly IShopService _shopService;

        public ShopController(
            IShopService shopService,
            ILogger<ShopController> log)
        {
            _log = log;
            _shopService = shopService;
        }

        [HttpGet]
        [Route("{id}/article")]
        public async Task<IActionResult> GetArticle([FromRoute] int id, int maxExpectedPrice = 200)
        {
            _log.LogTrace("Article retrieval started started.");
            var articleResult = await _shopService.GetArticle(id, maxExpectedPrice);
            _log.LogTrace("Article retrieval finished.");
            return GenerateResponse(articleResult);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyArticle([FromBody] ArticleDto article)
        {
            _log.LogTrace("Buying article started.");
            int userID = 0;
            var identityClaim = User.Claims.FirstOrDefault(x => x.Type == "UserID");
            if (int.TryParse(identityClaim.Value, out userID))
            {
                var articleResult = await _shopService.BuyArticle(article, userID);
                _log.LogTrace("Buying article finished.");
                return GenerateResponse(articleResult);
            }

            _log.LogError("Failed to buy article due to identity claim parsing error.");
            return BadRequest("Failed to parse identity claim.");
        }
    }
}