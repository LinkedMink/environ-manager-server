using System;
using LinkedMink.Data.Base;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class PageCriteriaViewModel
    {
        public const int DefaultElementsPerPage = 20;

        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public int SelectedPage => Page ?? 0;
        public int SelectedPageSize => PageSize ?? DefaultElementsPerPage;

        public static PageCriteria ToModel(PageCriteriaViewModel viewModel, Type entityType)
        {
            var criteria = new PageCriteria()
            {
                PageSize = viewModel.SelectedPageSize,
                PageNumber = viewModel.SelectedPage
            };

            return criteria;
        }
    }
}
