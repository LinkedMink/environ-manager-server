using System.ComponentModel.DataAnnotations;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Domain.EnvironManager.Entities;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public string RegistrationCode { get; set; }

        public ClientUser ToModel(ClientUser model = null)
        {
            model = model ?? new ClientUser();

            model.UserName = Username;
            model.Email = Email;
            model.PasswordHash = Password;

            return model;
        }
    }
}
