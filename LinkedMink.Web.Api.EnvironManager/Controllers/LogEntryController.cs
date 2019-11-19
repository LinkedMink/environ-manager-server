using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Data.Domain.EnvironManager.Repositories;
using LinkedMink.Web.EnvironManager.ViewModels;
using LinkedMink.Web.Infastructure;
using LinkedMink.Web.Infastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Web.Api.EnvironManager.Controllers
{
    [Produces("application/json")]
    [Route("log-entry")]
    public class LogEntryController : BaseController<LogEntryController>
    {
        public LogEntryController(
            BaseDbContext<ClientUser> context,
            ILogger<LogEntryController> logger,
            ILogEntryRepository repository) : base(context, logger)
        {
            _repository = repository;
        }

        [HttpPost("{hardwareDeviceId}")]
        public async Task<IActionResult> Get(long hardwareDeviceId, [FromBody] PageCriteriaViewModel viewModel)
        {
            return await ExecuteAsync(async () =>
            {
                var pageViewModel = PageCriteriaViewModel.ToModel(viewModel, typeof(LogEntry));
                var pagedData = await _repository.GetByDeviceAsync(hardwareDeviceId, pageViewModel);
                return PagedDataViewModel<LogEntryViewModel>.ToViewModel(pagedData);
            });
        }

        private readonly ILogEntryRepository _repository;
    }
}
