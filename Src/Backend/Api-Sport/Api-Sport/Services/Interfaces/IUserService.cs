using Api_Sport.Models;
using Api_Sport.Models.Dtos;

namespace Api_Sport.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string userName);
        Task<User> ValidationCredentialAsync(string userName,string email);
        Task<User> AddUserAsync(UserDto userDto);
    }
}
