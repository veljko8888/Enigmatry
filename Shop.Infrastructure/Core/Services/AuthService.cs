using Microsoft.IdentityModel.Tokens;
using Shared.Models.Contracts;
using Shared.Models.Models;
using Shared.Models.ResponseBuilder;
using Shop.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Core.Services
{
    public class AuthService : IAuthService
    {
        public bool CheckPasscode(string passcode, AppSettings appSettings)
        {
            if (passcode == appSettings.AuthPasscode)
            {
                return true;
            }

            return false;
        }

        public string GetToken(int userId, AppSettings appSettings)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID", userId.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
