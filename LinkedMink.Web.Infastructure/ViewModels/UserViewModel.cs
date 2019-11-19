using System.ComponentModel.DataAnnotations;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Web.Infastructure.ValidationAttributes;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [RequiredIfNew("UserId")]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public bool IsLocked { get; set; }

        public bool IsEmailVerified { get; set; }

        [Required]
        public string[] Roles { get; set; }

        public string Subscription { get; set; }

        public static ClientUser ToModel(UserViewModel viewModel, ClientUser model = null)
        {
            if (model == null)
            {
                model = new ClientUser();
            }
            /*
            model.Name = viewModel.Name;
            model.Email = viewModel.Email;

            if (!string.IsNullOrEmpty(viewModel.Password))
            {
                model.Password = viewModel.Password;
            }

            model.IsLocked = viewModel.IsLocked;
            model.IsEmailVerified = viewModel.IsEmailVerified;
            model.RoleNames = viewModel.Roles;
            model.SubscriptionName = viewModel.Subscription;
            */
            return model;
        }

        public static UserViewModel ToViewModel(ClientUser model)
        {
            UserViewModel viewModel = new UserViewModel
            {
                //UserId = model.UserId,
                //Name = model.Name,
                Email = model.Email,
                //IsLocked = model.IsLocked,
                //IsEmailVerified = model.IsEmailVerified,
                //Roles = model.RoleNames,
                //Subscription = model.SubscriptionName
            };
            return viewModel;
        }
    }
}
