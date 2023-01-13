using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Models.Contracts;
using Shared.Models.Models;
using Shared.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vendor.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> Authorize(GetTokenDto tokenDto)
        {
            var isPasscodeCorrect = _authService.CheckPasscode(tokenDto.Passcode, _appSettings);
            if (isPasscodeCorrect)
            {
                var dummyShopCompanyId = new Random().Next(1, 1000);
                string token = _authService.GetToken(dummyShopCompanyId, _appSettings);
                return Ok(new { token });
            }
            else
            {
                return BadRequest("Wrong Data");
            }
        }
    }
}
