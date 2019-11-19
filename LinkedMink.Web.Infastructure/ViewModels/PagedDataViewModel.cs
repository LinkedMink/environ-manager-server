using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LinkedMink.Data.Base;

namespace LinkedMink.Web.Infastructure.ViewModels
{
    public class PagedDataViewModel<T> : RecordListViewModel<T>
    {
        public int PageSelected { get; set; }

        public int PageTotal { get; set; }

        public long RecordsTotal { get; set; }

        public string SortAttribute { get; set; }

        public int Order { get; set; }

        public static PagedDataViewModel<T> ToViewModel<TIn>(PagedData<TIn> data)
        {
            PagedDataViewModel<T> viewModel = new PagedDataViewModel<T>
            {
                PageSelected = data.Criteria.PageNumber,
                //SortAttribute = data.SortAttribute,
                //Order = (int) data.Order,
                Records = data.RecordsInPage
                    .Select(e => (T) ToViewModelMethods[typeof(T)].Invoke(null, new object[] {e}))
                    .ToArray(),
                PageTotal = data.NumberOfPages,
                RecordsTotal = data.NumberOfRecords
            };

            return viewModel;
        }
    }
}
