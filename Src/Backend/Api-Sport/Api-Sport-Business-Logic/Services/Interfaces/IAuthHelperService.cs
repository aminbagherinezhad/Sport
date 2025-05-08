using Api_Sport_Business_Logic.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Sport_Business_Logic_Business_Logic.Services.Interfaces
{
    public interface IAuthHelperService
    {
        Task<bool> UserExistsAsync(string username, string email);
        Task<string> GenerateJwtTokenAsync(UserDto user);
    }
}
