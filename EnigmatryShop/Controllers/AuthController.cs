using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Models.Contracts;
using Shared.Models.Models;
using Shared.Models.Shop;
using Shop.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _log;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> log,
            IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _log = log;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> Authorize(GetTokenDto tokenDto)
        {
            _log.LogTrace("Client authorization started.");
            var isPasscodeCorrect = _authService.CheckPasscode(tokenDto.Passcode, _appSettings);
            if (isPasscodeCorrect)
            {
                var dummyUserId = new Random().Next(1, 1000);
                string token = _authService.GetToken(dummyUserId, _appSettings);
                _log.LogTrace("Successfully authorized client.");
                return Ok(new { token });
            }
            else
            {
                _log.LogError("Failed to authorize client.");
                return BadRequest("Wrong Data");
            }
        }
    }
}
