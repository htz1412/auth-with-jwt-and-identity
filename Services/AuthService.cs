using AuthImplementation.DataModels;
using AuthImplementation.DTOs;
using AuthImplementation.Entities;
using AuthImplementation.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public async Task<UserDataModel> Login(UserForAuthenticationDto userForAuthentication)
        {
            var user = await Validate(userForAuthentication);
            UserDataModel userDataModel = null;

            if (user != null)
            {
                userDataModel = new UserDataModel();
                _mapper.Map(user, userDataModel);
                var signingCredentials = GetSigningCredentials();
                var claims = await GetClaimsForUser(user);
                var token = GenerateTokenOptions(signingCredentials, claims);
                userDataModel.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return userDataModel;
        }

        private async Task<User> Validate(UserForAuthenticationDto userForAuthentication)
        {
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, userForAuthentication.Password);
                return isValid ? user : null;
            }

            return user;
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
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"], 
                claims: claims, 
                notBefore: DateTime.UtcNow, 
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])), 
                signingCredentials: signingCredentials
            );
            return jwtSecurityToken;
        }
    }
}
