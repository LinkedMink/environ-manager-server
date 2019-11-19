using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Net.Message;
using LinkedMink.Web.Infastructure.Options;

namespace LinkedMink.Web.Infastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public class AuthenticationResult
        {
            public long UserId { get; set; }

            public string Token { get; set; }

            public IList<string> Roles { get; set; }
        }

        public AuthenticationService(
            IOptionsSnapshot<AuthenticationOptions> options, 
            ILogger<AuthenticationService> logger, 
            UserManager<ClientUser> userManager, 
            SignInManager<ClientUser> signInManager,
            IEmailService emailService)
        {
            _options = options.Value;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<ServiceResult<AuthenticationResult>> AuthenticateAsync(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);
                var roles = await _userManager.GetRolesAsync(user);

                var token = GetSecurityToken(user);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogDebug($"Authenticated: {username}, Token: {tokenString}");

                return new ServiceResult<AuthenticationResult>()
                {
                    Data = new AuthenticationResult
                    {
                        Token = tokenString,
                        UserId = user.Id,
                        Roles = roles
                    }
                };
            }
            else if (result.IsLockedOut)
                return new ServiceResult<AuthenticationResult> { Code = ServiceResultCode.IsLockedOut };
            else if (result.IsNotAllowed || result.RequiresTwoFactor)
                return new ServiceResult<AuthenticationResult> { Code = ServiceResultCode.IsInactive };
            else
                return new ServiceResult<AuthenticationResult> { Code = ServiceResultCode.Failed };
        }

        public async Task<string> CreatePasswordResetCodeAsync(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> ResetPasswordAsync(string username, string password, string resetCode)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var result = await _userManager.ResetPasswordAsync(user, resetCode, password);

            if (result.Succeeded)
                return true;

            return false;
        }

        private SecurityToken GetSecurityToken(ClientUser user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_options.DaysToExpire),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(GetSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return token;

            SymmetricSecurityKey GetSecurityKey()
            {
                var key = Encoding.UTF8.GetBytes(_options.SigningKey);
                return new SymmetricSecurityKey(key);
            }
        }

        private readonly AuthenticationOptions _options;
        private readonly ILogger _logger;
        private readonly UserManager<ClientUser> _userManager;
        private readonly SignInManager<ClientUser> _signInManager;
        private readonly IEmailService _emailService;
    }
}
