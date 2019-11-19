using System.Threading.Tasks;

namespace LinkedMink.Web.Infastructure.Services
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<AuthenticationService.AuthenticationResult>> AuthenticateAsync(string username, string password);
        Task<string> CreatePasswordResetCodeAsync(string username);
        Task<bool> ResetPasswordAsync(string username, string password, string resetCode);
    }
}
