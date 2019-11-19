using System;
using LinkedMink.Data.Base.Entities;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public abstract class TrackedEntityViewModel : BaseEntityViewModel
    {
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public abstract class TrackedEntityViewModel<TViewModel, TEntity> : TrackedEntityViewModel
        where TViewModel : TrackedEntityViewModel, new()
        where TEntity : TrackedEntity
    {
        public static TViewModel ToViewModel(TrackedEntity entity)
        {
            var viewModel = BaseEntityViewModel<TViewModel, TrackedEntity>
                .ToViewModel(entity);

            viewModel.ModifiedBy = entity.ModifiedBy;
            viewModel.ModifiedDateTime = entity.ModifiedDateTime;
            viewModel.CreatedBy = entity.CreatedBy;
            viewModel.CreatedDateTime = entity.CreatedDateTime;

            return viewModel;
        }
    }
}
