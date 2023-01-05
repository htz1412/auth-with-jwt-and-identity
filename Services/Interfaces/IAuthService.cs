using AuthImplementation.DataModels;
using AuthImplementation.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AuthImplementation.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> Register(UserForRegistrationDto userForRegistration);
        Task<UserDataModel> Login(UserForAuthenticationDto userForAuthentication, bool populateRefreshTokenExpiryTime = false);
        Task<UserDataModel> RefreshToken(RefreshTokenDto refreshTokenDto);
    }
}
