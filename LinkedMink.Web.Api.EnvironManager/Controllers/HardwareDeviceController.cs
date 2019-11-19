using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Web.EnvironManager.Services;
using LinkedMink.Web.EnvironManager.ViewModels;
using LinkedMink.Web.Infastructure;
using LinkedMink.Web.Infastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Web.Api.EnvironManager.Controllers
{
    [Produces("application/json")]
    [Route("hardware-device")]
    public class HardwareDeviceController : BaseController<LogEntryController>
    {
        public HardwareDeviceController(
            BaseDbContext<ClientUser> context,
            ILogger<LogEntryController> logger,
            IRepository<HardwareDevice> repository,
            IHardwareDeviceStatusService statusService) : base(context, logger)
        {
            _repository = repository;
            _statusService = statusService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await ExecuteAsync(async () =>
            {
                var pagedData = await _repository.GetAsync();
                return RecordListViewModel<HardwareDeviceViewModel>.ToViewModel(pagedData);
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return await ExecuteAsync(async () =>
            {
                var record = await _repository.GetAsync(id);
                var output = HardwareDeviceViewModel.ToViewModel(record);
                output.Status = await _statusService.GetStatusAsync(record.Host, record.Port);
                return output;
            });
        }

        private readonly IRepository<HardwareDevice> _repository;
        private readonly IHardwareDeviceStatusService _statusService;
    }
}
