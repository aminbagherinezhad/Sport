using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_DataLayer.Models;

namespace Api_Sport_Business_Logic.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByUsernameAsync(string userName);
        Task<User> ValidationCredentialAsync(string userName,string email);
        Task<User> AddUserAsync(UserDto userDto);
    }
}
