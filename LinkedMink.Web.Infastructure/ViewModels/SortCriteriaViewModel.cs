using LinkedMink.Data.Base;
using System;
using System.Linq;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class SortCriteriaViewModel
    {
        public string SortBy { get; set; }
        public int? Order { get; set; }

        public string SortAttribute => !string.IsNullOrEmpty(SortBy) ?
            SortBy.First().ToString().ToUpper() + SortBy.Substring(1) : null;

        public SortCriteria.SortOrder SortOrder => Order.HasValue ?
            (SortCriteria.SortOrder)Order : SortCriteria.SortOrder.Descending;

        public static SortCriteria ToModel(SortCriteriaViewModel viewModel, Type entityType)
        {
            var sortAttribute = viewModel.SortAttribute;
            if (!string.IsNullOrEmpty(sortAttribute))
            {
                var sortProperty = entityType.GetProperty(sortAttribute);
                if (sortProperty != null)
                {
                    return new SortCriteria()
                    {
                        Order = viewModel.SortOrder,
                        Property = sortProperty
                    };
                }
            }

            return null;
        }
    }
}
