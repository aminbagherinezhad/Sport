using Api_Sport.Models;
using Api_Sport.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticateController(SportDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto
         user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User res_UserValid = await ValidationCredentialAsync(user.UserName, user.Email);
            if (res_UserValid != null)
            {
                // Find user Has Before
                return BadRequest();
            }
            //Add User
            var createdUser = await AddUserAsync(user);
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
            return Ok(tokenToReturn);
        }

        #region method

        public async Task<User> ValidationCredentialAsync(string userName, string email)
        {
            userName = userName?.Trim().ToLower();
            email = email?.Trim().ToLower();

            return await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u =>
                    u.UserName.ToLower() == userName ||
                    (!string.IsNullOrEmpty(email) && u.Email.ToLower() == email));
        }
        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var newUser = _mapper.Map<User>(userDto);

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
        #endregion
    }
}
