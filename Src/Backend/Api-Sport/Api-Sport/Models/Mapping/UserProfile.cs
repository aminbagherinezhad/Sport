using Api_Sport.Models.Dtos;
using AutoMapper;

namespace Api_Sport.Models.Mapping
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
