using System.Threading.Tasks;
using LinkedMink.Web.Infastructure.ViewModels;

namespace LinkedMink.Web.Infastructure.Services
{
    public interface IUserService
    {
        Task<ServiceResult<AuthenticationService.AuthenticationResult>> RegisterAsync(RegisterViewModel viewModel);
    }
}
