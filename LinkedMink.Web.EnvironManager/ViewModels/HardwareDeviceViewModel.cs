using System;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Web.Infastructure.ViewModels;

namespace LinkedMink.Web.EnvironManager.ViewModels
{
    public class HardwareDeviceViewModel : TrackedEntityViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public ushort Port { get; set; }
        public HardwareDevice.Type DeviceType { get; set; }
        public DateTime LastUpdateReceived { get; set; }
        public HardwareDeviceStatusViewModel Status { get; set; }

        public static HardwareDeviceViewModel ToViewModel(HardwareDevice entity)
        {
            var viewModel = TrackedEntityViewModel<HardwareDeviceViewModel, HardwareDevice>
                .ToViewModel(entity);

            viewModel.Name = entity.Name;
            viewModel.Description = entity.Description;
            viewModel.Host = entity.Host;
            viewModel.Port = entity.Port;
            viewModel.DeviceType = entity.DeviceType;
            viewModel.LastUpdateReceived = entity.LastUpdateReceived;

            return viewModel;
        }
    }
}
