using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Net.Message;
using LinkedMink.Web.Infastructure.ViewModels;
using System.Threading.Tasks;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Domain.EnvironManager.Entities;

namespace LinkedMink.Web.Infastructure.Services
{
    public class UserService : IUserService
    {
        public UserService(
            IOptionsSnapshot<IdentityOptions> options,
            ILogger<UserService> logger,
            UserManager<ClientUser> userManager,
            IAuthenticationService authenticationService,
            IEmailService emailService)
        {
            _options = options.Value;
            _logger = logger;
            _userManager = userManager;
            _authenticationService = authenticationService;
            _emailService = emailService;
        }

        public async Task<ServiceResult<AuthenticationService.AuthenticationResult>> RegisterAsync(RegisterViewModel viewModel)
        {
            if (!RegistrationCodes.Contains(viewModel.RegistrationCode))
                return new ServiceResult<AuthenticationService.AuthenticationResult>
                {
                    Code = ServiceResultCode.RegistrationCodeIncorrect
                };

            var user = viewModel.ToModel();
            if (string.IsNullOrEmpty(user.Email))
                user.Email = DummyEmail;

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                return new ServiceResult<AuthenticationService.AuthenticationResult>
                {
                    Code = ServiceResultCode.Failed,
                    Errors = result.Errors.ToArray()
                };

            result = await _userManager.AddToRoleAsync(user, PublicUserRole);
            if (!result.Succeeded)
                return new ServiceResult<AuthenticationService.AuthenticationResult>
                {
                    Code = ServiceResultCode.Failed,
                    Errors = result.Errors.ToArray()
                };

            return await _authenticationService.AuthenticateAsync(viewModel.Username, viewModel.Password);
        }

        private readonly IdentityOptions _options;
        private readonly ILogger _logger;
        private readonly UserManager<ClientUser> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;

        private const string PublicUserRole = "Public";
        private const string DummyEmail = "anonymous@returntosender.space";

        private static readonly HashSet<string> RegistrationCodes = new HashSet<string> {
            "k4rblUPmJP6NlgcK",
            "LT4i7pRyfurtJYz3",
            "dbY5Z9JnOBo8B5zu",
            "2IYojy0mqOXv3qnf"
        };
    }
}
