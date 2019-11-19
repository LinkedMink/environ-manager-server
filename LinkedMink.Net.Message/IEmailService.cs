using System.Threading.Tasks;

namespace LinkedMink.Net.Message
{
    public interface IEmailService
    {
        Task SendAdministratorEmailAsync(string subject, string message);

        Task SendEmailAsync(string toAddress, string subject, string message);

        Task SendEmailAsync(string toAddress, string subject, string html, string alternative);
    }
}
