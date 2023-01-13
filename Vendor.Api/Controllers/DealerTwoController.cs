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
            var articleResult = await _dealerService.GetArticle(id, 0, false);
            return GenerateResponse(articleResult);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyArticle([FromBody] ArticleDto article)
        {
            int partnerCompanyID = 0;
            var identityClaim = User.Claims.FirstOrDefault(x => x.Type == "CompanyID");
            if (int.TryParse(identityClaim.Value, out partnerCompanyID))
            {
                var articleResult = await _dealerService.BuyArticle(article, partnerCompanyID, false);
                return GenerateResponse(articleResult);
            }

            return BadRequest("Failed to parse identity claim.");
        }
    }
}