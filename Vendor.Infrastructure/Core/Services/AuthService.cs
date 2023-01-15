using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Contracts;
using Shared.Models.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Vendor.Infrastructure.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _log;

        public AuthService(
            ILogger<AuthService> log)
        {
            _log = log;
        }

        public bool CheckPasscode(string passcode, AppSettings appSettings)
        {
            _log.LogTrace("Checking passcode started.");
            if (passcode == appSettings.AuthPasscode)
            {
                _log.LogTrace("Passcode correct.");
                return true;
            }

            _log.LogTrace("Passcode invalid.");
            return false;
        }

        public string GetToken(int companyId, AppSettings appSettings)
        {
            _log.LogTrace($"Get token started.");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("CompanyID", companyId.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            _log.LogTrace($"Get token finished. Token value: {token}");
            return token;
        }
    }
}
