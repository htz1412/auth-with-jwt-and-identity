using AuthImplementation.DTOs;
using AuthImplementation.Entities;
using AuthImplementation.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthImplementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            }
            return result;
        }
    }
}
