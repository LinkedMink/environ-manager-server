using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Web.Infastructure;
using LinkedMink.Web.Infastructure.Services;
using LinkedMink.Web.Infastructure.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Web.Api.EnvironManager.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("password")]
    public class PasswordController : BaseController<PasswordController>
    {
        public PasswordController(
            BaseDbContext<ClientUser> context,
            ILogger<PasswordController> logger,
            IAuthenticationService authenticationService) : base(context, logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            return await ExecuteAsync(() =>
            {
                return _authenticationService.CreatePasswordResetCodeAsync(email);
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PasswordResetViewModel viewModel)
        {
            return await ExecuteAsync(() =>
            {
                return _authenticationService.ResetPasswordAsync(viewModel.Email, viewModel.Password, viewModel.ResetCode);
            });
        }

        private readonly IAuthenticationService _authenticationService;
    }
}