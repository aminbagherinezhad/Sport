using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_Business_Logic.Services.Interfaces;
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
using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;

namespace Api_Sport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        IConfiguration _configuration;
        private readonly IAuthHelperService _authHelper;
        private readonly IMapper _mapper;

        public AuthenticateController(IConfiguration configuration, IMapper mapper, IUserService userService, IAuthHelperService authHelper)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _authHelper = authHelper;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("ModelState Not Valid.");
                return BadRequest(ModelState);

            }

            if (await _authHelper.UserExistsAsync(user.UserName, user.Email))
            {
                Log.Warning("User already exists.");
                return BadRequest("User already exists.");
            }

            var createdUser = await _userService.AddUserAsync(user);
            var userDto = _mapper.Map<UserDto>(createdUser);
            var token = await _authHelper.GenerateJwtTokenAsync(userDto);
            Log.Information("SuccsessFully Generate Token");

            return Ok(new { token });
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

            var userDto = _mapper.Map<UserDto>(user);
            var token = await _authHelper.GenerateJwtTokenAsync(userDto);

            Log.Information("User logged in and token issued");
            return Ok(new { token });
        }
    }
}
