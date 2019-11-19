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
    [Route("register")]
    public class RegisterController : BaseController<RegisterController>
    {
        public RegisterController(
            BaseDbContext<ClientUser> context,
            ILogger<RegisterController> logger,
            IUserService userService) : base(context, logger)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel viewModel)
        {
            return await ExecuteAsync(() =>
            {
                return _userService.RegisterAsync(viewModel);
            });
        }

        private readonly IUserService _userService;
    }
}
