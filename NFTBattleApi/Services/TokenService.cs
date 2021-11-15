using System;
using Microsoft.IdentityModel.Tokens;
using NFTBattleApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NFTBattleApi.Services
{
    public class TokenService
    {
        private readonly ITokenSettings _tokenSettings;

        public TokenService(ITokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }


        private TimeSpan ExpiryDuration = new TimeSpan(8, 0, 0);
        public string BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, user.Id),
             };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_tokenSettings.JwtIssuer, _tokenSettings.JwtIssuer, claims,
                expires: DateTime.Now.Add(ExpiryDuration), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
