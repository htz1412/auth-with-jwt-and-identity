using AuthImplementation.DataModels;
using AuthImplementation.DTOs;
using AuthImplementation.Entities;
using AuthImplementation.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthImplementation.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IConfiguration configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<IdentityResult> Register(UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
            }
            return result;
        }

        public async Task<UserDataModel> Login(UserForAuthenticationDto userForAuthentication, bool populateRefreshTokenExpiryTime = false)
        {
            var user = await Validate(userForAuthentication);

            if (user == null)
            {
                return null;
            }

            var userDataModel = await CreateTokenForUser(user, false);
            return userDataModel;
        }

        public async Task<UserDataModel> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var claimPrinciple = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);

            if(claimPrinciple == null)
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(claimPrinciple.Identity.Name);

            if(user == null)
            {
                return null;
            }

            var userDataModel = await CreateTokenForUser(user);
            return userDataModel;
        }

        private async Task<UserDataModel> CreateTokenForUser(User user, bool populateRefreshTokenExpiryTime = false)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsForUser(user);
            var token = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();

            var userDataModel = _mapper.Map<UserDataModel>(user);
            userDataModel.RefreshToken = refreshToken;
            userDataModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);

            user.RefreshToken = refreshToken;

            if (populateRefreshTokenExpiryTime)
            {
                var expiresInDays = Convert.ToDouble(_configuration["RefreshTokenExpiresInDays"]);
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(expiresInDays);
            }

            await _userManager.UpdateAsync(user);

            return userDataModel;
        }

        private async Task<User> Validate(UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            if (user == null)
            {
                return null;
            }

            var isValid = await _userManager.CheckPasswordAsync(user, userForAuthentication.Password);
            return isValid ? user : null;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var signInKey = Encoding.UTF8.GetBytes(_configuration["JWT:SignInKey"]);
            var securityKey = new SymmetricSecurityKey(signInKey);
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsForUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var jwtSecurityToken = new JwtSecurityToken
            (
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials
            );
            return jwtSecurityToken;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var signInKey = jwtSettings["SignInKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signInKey)),
                ClockSkew = TimeSpan.Zero
            };

            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return claimsPrincipal;
        }
    }
}
