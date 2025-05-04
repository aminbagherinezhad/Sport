using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Sport_Business_Logic.Services
{
    public class UserService : IUserService
    {
        private readonly SportDbContext _context;
        public UserService(SportDbContext context)
        {
            _context = context;
        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password)
            };

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.UserName == userName);
        }

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
    }
}
