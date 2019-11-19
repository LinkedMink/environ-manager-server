using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Web.Infastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Web.Api.EnvironManager.Controllers
{
    [Produces("application/json")]
    [Route("account")]
    public class AccountController : BaseController<AccountController>
    {
        public AccountController(
            BaseDbContext<ClientUser> context,
            ILogger<AccountController> logger,
            IRepository<ClientUser> repository) : base(context, logger)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ExecuteAsync(() =>
            {
                if (CurrentUserId.HasValue)
                    return _repository.GetAsync(CurrentUserId.Value);

                return Task.FromResult<ClientUser>(null);
            });
        }

        private readonly IRepository<ClientUser> _repository;
    }
}