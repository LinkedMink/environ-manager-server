using LinkedMink.Data.Base.Entities;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public abstract class BaseEntityViewModel
    {
        public long Id { get; set; }
    }

    public abstract class BaseEntityViewModel<TViewModel, TEntity> : BaseEntityViewModel
        where TViewModel : BaseEntityViewModel, new()
        where TEntity : BaseEntity
    {
        public static TViewModel ToViewModel(TEntity entity)
        {
            TViewModel viewModel = new TViewModel
            {
                Id = entity.Id
            };

            return viewModel;
        }
    }
}
