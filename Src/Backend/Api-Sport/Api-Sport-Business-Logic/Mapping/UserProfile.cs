using Api_Sport_Business_Logic.Models.Dtos;
using Api_Sport_DataLayer.Models;
using AutoMapper;

namespace Api_Sport_Business_Logic.Models.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Mapping از Dto به Entity و برعکس
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
