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

namespace Vendor.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DealerTwoController : BaseApiController
    {
        private readonly ILogger<DealerTwoController> _log;
        private readonly IShopService _dealerService;

        public DealerTwoController(
            IShopService dealerService,
            ILogger<DealerTwoController> log)
        {
            _log = log;
            _dealerService = dealerService;
        }

        [HttpGet]
        [Route("{id}/article")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            _log.LogTrace("Get article on dealer 2 api started.");
            var articleResult = await _dealerService.GetArticle(id, 0, false);
            _log.LogTrace("Get article on dealer 2 api finished.");
            return GenerateResponse(articleResult);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyArticle([FromBody] ArticleDto article)
        {
            _log.LogTrace("Buy article on dealer 2 api started.");
            int partnerCompanyID = 0;
            var identityClaim = User.Claims.FirstOrDefault(x => x.Type == "CompanyID");
            if (int.TryParse(identityClaim.Value, out partnerCompanyID))
            {
                var articleResult = await _dealerService.BuyArticle(article, partnerCompanyID, false);
                _log.LogTrace("Buy article on dealer 2 api finished.");
                return GenerateResponse(articleResult);
            }

            _log.LogError("Buy article on dealer 2 api failed due to identity claim parsing fail.");
            return BadRequest("Failed to parse identity claim.");
        }
    }
}