using System.ComponentModel.DataAnnotations;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Domain.EnvironManager.Entities;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class AccountViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public bool IsEmailVerified { get; set; }

        public static ClientUser ToModel(AccountViewModel viewModel, ClientUser model = null)
        {
            if (model == null)
            {
                model = new ClientUser();
            }
            /*
            model.UserName = viewModel.Name;


            if (model.Email != viewModel.Email)
            {
                model.IsEmailVerified = false;
            }


            model.Email = viewModel.Email;

            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                model.Password = viewModel.Password;
            }
            */
            return model;
        }

        public static AccountViewModel ToViewModel(ClientUser user)
        {
            AccountViewModel viewModel = new AccountViewModel
            {
                Name = user.UserName,
                Email = user.Email,
                //IsEmailVerified = user.IsEmailVerified
            };
            return viewModel;
        }
    }
}
