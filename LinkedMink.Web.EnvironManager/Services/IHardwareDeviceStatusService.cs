using System.Threading.Tasks;
using LinkedMink.Web.EnvironManager.ViewModels;

namespace LinkedMink.Web.EnvironManager.Services
{
    public interface IHardwareDeviceStatusService
    {
        Task<HardwareDeviceStatusViewModel> GetStatusAsync(string host, ushort port);
    }
}
