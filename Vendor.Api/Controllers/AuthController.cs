using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthController> _log;

        public AuthController(
            IAuthService authService,
            IOptions<AppSettings> appSettings,
            ILogger<AuthController> log)
        {
            _authService = authService;
            _appSettings = appSettings.Value;
            _log = log;
        }

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> Authorize(GetTokenDto tokenDto)
        {
            _log.LogTrace("Shop authorization on Vendor API started.");
            var isPasscodeCorrect = _authService.CheckPasscode(tokenDto.Passcode, _appSettings);
            if (isPasscodeCorrect)
            {
                var dummyShopCompanyId = new Random().Next(1, 1000);
                string token = _authService.GetToken(dummyShopCompanyId, _appSettings);
                _log.LogTrace("Shop authorization on Vendor API finished.");
                return Ok(new { token });
            }
            else
            {
                _log.LogError("Shop authorization on Vendor API failed.");
                return BadRequest("Wrong Data");
            }
        }
    }
}
