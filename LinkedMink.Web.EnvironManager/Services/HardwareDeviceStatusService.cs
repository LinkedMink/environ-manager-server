using System.Net.Http;
using System.Threading.Tasks;
using LinkedMink.Web.EnvironManager.ViewModels;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Web.EnvironManager.Services
{
    public class HardwareDeviceStatusService : IHardwareDeviceStatusService
    {
        public HardwareDeviceStatusService(
            ILogger<HardwareDeviceStatusService> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task<HardwareDeviceStatusViewModel> GetStatusAsync(string host, ushort port)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://{host}:{port}/status");

            try
            {
                using (var client = _clientFactory.CreateClient())
                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return new HardwareDeviceStatusViewModel()
                        {
                            IsOnline = true,
                            Response = await response.Content.ReadAsStringAsync()
                        };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.ToString());
            }

            return new HardwareDeviceStatusViewModel()
            {
                IsOnline = false,
                Response = null
            };
        }

        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;
    }
}
