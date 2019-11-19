using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using Newtonsoft.Json;

namespace LinkedMink.Processor.EnvironManager.Tools.Commands
{
    internal class SeedCommand : ICommand
    {
        public async Task<CommandStatus> ExecuteAsync(ToolApplicationContext context)
        {
            var logger = (ILogger<SeedCommand>)context.ServiceProvider.GetService(typeof(ILogger<SeedCommand>));
            var userManager = (UserManager<ClientUser>)context.ServiceProvider.GetService(typeof(UserManager<ClientUser>));
            var roleManager = (RoleManager<ClientRole>)context.ServiceProvider.GetService(typeof(RoleManager<ClientRole>));

            var seedData = File.ReadAllText(SeedFileName);
            var seedRecords = JsonConvert.DeserializeObject<SeedData>(seedData);

            ClientRole[] roles =
            {
                new ClientRole() { Name = "Administrator" },
                new ClientRole() { Name = "Public" }
            };

            foreach (var role in roles)
            {
                logger.LogDebug($"Creating Role: {role.Name}");
                var result = roleManager.CreateAsync(role).Result;
                ShowErrors(result);
            }

            foreach (var userSeed in seedRecords.Users)
            {
                var user = new ClientUser()
                {
                    UserName = userSeed.Username,
                    Email = userSeed.Email,
                    PasswordHash = userSeed.Password,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                logger.LogDebug($"Creating User: {user.UserName}");
                var result = await userManager.CreateAsync(user, user.PasswordHash);
                ShowErrors(result);
                result = await userManager.AddToRoleAsync(user, "Administrator");
                ShowErrors(result);
            }

            var devices = seedRecords.Devices
                .Select(deviceSeed => new HardwareDevice()
                {
                    Name = deviceSeed.Name,
                    Description = deviceSeed.Host,
                    DeviceType = HardwareDevice.Type.RaspberryPi,
                    Host = deviceSeed.Host,
                    Port = deviceSeed.Port,
                    LastUpdateReceived = DateTime.Now
                });

            var hardwareDeviceRepository = (IRepository<HardwareDevice>)context.ServiceProvider
                .GetService(typeof(IRepository<HardwareDevice>));
            await hardwareDeviceRepository.AddAsync(devices);

            void ShowErrors(IdentityResult result)
            {
                if (result.Errors.Any())
                    foreach (var error in result.Errors)
                        logger.LogWarning($"Code: {error.Code}, Description: {error.Description}");
            }

            return CommandStatus.Succeeded;
        }

        private class SeedData
        {
            public UserRecord[] Users { get; set; }
            public DeviceRecord[] Devices { get; set; }
        }

        private class UserRecord
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private class DeviceRecord
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Host { get; set; }
            public ushort Port { get; set; }
        }

        private const string SeedFileName = "seed.json";
    }
}
