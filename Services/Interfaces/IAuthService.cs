using AuthImplementation.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthImplementation.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistration);
    }
}
