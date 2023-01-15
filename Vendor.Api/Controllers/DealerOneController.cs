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
    public class DealerOneController : BaseApiController
    {
        private readonly ILogger<DealerOneController> _log;
        private readonly IShopService _dealerService;

        public DealerOneController(
            IShopService dealerService,
            ILogger<DealerOneController> log)
        {
            _log = log;
            _dealerService = dealerService;
        }

        [HttpGet]
        [Route("{id}/article")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            _log.LogTrace("Get article on dealer 1 api started.");
            var articleResult = await _dealerService.GetArticle(id);
            _log.LogTrace("Get article on dealer 1 api finished");
            return GenerateResponse(articleResult);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyArticle([FromBody] ArticleDto article)
        {
            _log.LogTrace("Buy article on dealer 1 api started.");
            int partnerCompanyID = 0;
            var identityClaim = User.Claims.FirstOrDefault(x => x.Type == "CompanyID");
            if (int.TryParse(identityClaim.Value, out partnerCompanyID))
            {
                var articleResult = await _dealerService.BuyArticle(article, partnerCompanyID);
                _log.LogTrace("Buy article on dealer 1 api finished.");
                return GenerateResponse(articleResult);
            }

            _log.LogError("Buy article on dealer 1 api failed due to identity claim parsing fail.");
            return BadRequest("Failed to parse identity claim.");
        }
    }
}