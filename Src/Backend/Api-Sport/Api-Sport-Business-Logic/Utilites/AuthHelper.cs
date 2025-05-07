using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_Business_Logic.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api_Sport_Business_Logic_Business_Logic.Utilites
{
    public class AuthHelper
    {
        IConfiguration _configuration;

        private readonly IUserService _userService;
        public AuthHelper(IUserService userService, IConfiguration configuration)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<bool> UserExistsAsync(string username, string email)
        {
            var existingUser = await _userService.ValidationCredentialAsync(username, email);
            return existingUser != null;
        }

        public async Task<string> GenerateJwtTokenAsync(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
            );

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
            );

            var claims = new List<Claim>();

            if (user.Id != 0)
                claims.Add(new Claim("userId", user.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim("userName", user.UserName));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
