using AuthImplementation.DataModels;
using AuthImplementation.DTOs;
using AuthImplementation.Entities;
using AutoMapper;

namespace AuthImplementation.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserForRegistrationDto>();
            CreateMap<User, UserDataModel>();
        }
    }
}
