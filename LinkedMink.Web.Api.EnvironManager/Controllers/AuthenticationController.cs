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
    [Route("authentication")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        public AuthenticationController(
            BaseDbContext<ClientUser> context,
            ILogger<AuthenticationController> logger, 
            IAuthenticationService authenticationService) : base(context, logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginViewModel viewModel)
        {
            return await ExecuteAsync(() =>
            {
                return _authenticationService.AuthenticateAsync(viewModel.Email, viewModel.Password);
            });
        }

        private readonly IAuthenticationService _authenticationService;
    }
}