using System;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Web.Infastructure.ViewModels;

namespace LinkedMink.Web.EnvironManager.ViewModels
{
    public class LogEntryViewModel : BaseEntityViewModel
    {
        public decimal RelativeHumidity { get; set; }
        public decimal Temperature { get; set; }
        public DateTime RecordedOn { get; set; }

        public static LogEntryViewModel ToViewModel(LogEntry entity)
        {
            var viewModel = BaseEntityViewModel<LogEntryViewModel, LogEntry>
                .ToViewModel(entity);

            viewModel.RelativeHumidity = entity.RelativeHumidity;
            viewModel.Temperature = entity.Temperature;
            viewModel.RecordedOn = entity.RecordedOn;

            return viewModel;
        }
    }
}
