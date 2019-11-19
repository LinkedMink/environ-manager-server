using System.ComponentModel.DataAnnotations;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public string ResetCode { get; set; }
    }
}
