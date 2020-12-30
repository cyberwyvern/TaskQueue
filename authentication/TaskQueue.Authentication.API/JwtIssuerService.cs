using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskQueue.Authentication
{
    public sealed class JwtIssuerService
    {
        private readonly IConfiguration config;

        public JwtIssuerService(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["JwtSettings:EncryptionKey"]));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: config["JwtSettings:Issuer"],
                audience: config["JwtSettings:Audience"],
                notBefore: DateTime.UtcNow,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(int.Parse(config["JwtSettings:LifetimeSeconds"])),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}