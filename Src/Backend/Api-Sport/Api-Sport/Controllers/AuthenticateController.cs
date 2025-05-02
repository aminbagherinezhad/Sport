using Api_Sport.Models;
using Api_Sport.Models.Dtos;
using Api_Sport.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Sport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly SportDbContext _context;
        private readonly IUserService _userService;
        IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticateController(SportDbContext context, IConfiguration configuration, IMapper mapper, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto
         user)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Required Property Has Not Null");
                return BadRequest(ModelState);
            }

            User res_UserValid = await _userService.ValidationCredentialAsync(user.UserName, user.Email);
            if (res_UserValid != null)
            {
                Log.Warning("Find User Has Before");

                return BadRequest();
            }

            var createdUser = await _userService.AddUserAsync(user);
            Log.Information("Succsessfully Create User In DB");
            var userDto = _mapper.Map<UserDto>(createdUser); // برگرداندن مقدار جدید

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
                );
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );
            var claimsForToken = new List<Claim>();
            if (userDto.Id != 0)
                claimsForToken.Add(new Claim("userId", userDto.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(userDto.UserName))
                claimsForToken.Add(new Claim("userName", userDto.UserName));

            if (!string.IsNullOrWhiteSpace(userDto.Email))
                claimsForToken.Add(new Claim(ClaimTypes.Email, userDto.Email));


            var jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);
            Log.Information("Succsessfully Create Token For User");
            return Ok(tokenToReturn);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid login request");
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByUsernameAsync(loginDto.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                Log.Warning("Invalid username or password");
                return Unauthorized("Invalid credentials");
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
            );
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
            );

            var claimsForToken = new List<Claim>();
            if (user.Id != 0)
                claimsForToken.Add(new Claim("userId", user.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(user.UserName))
                claimsForToken.Add(new Claim("userName", user.UserName));

            if (!string.IsNullOrWhiteSpace(user.Email))
                claimsForToken.Add(new Claim(ClaimTypes.Email, user.Email));


            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddHours(1),
                signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            Log.Information("User logged in and token issued");

            return Ok(token);
        }

    }
}
