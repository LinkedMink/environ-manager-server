using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using LinkedMink.Data.Base.Entities;

namespace LinkedMink.Data.Domain.EnvironManager.Entities
{
    public class HardwareDevice : TrackedEntity
    {
        public enum Type
        {
            Undefined = 0,
            RaspberryPi
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Host { get; set; }
        public ushort Port { get; set; } 
        public Type DeviceType { get; set; }
        public DateTime LastUpdateReceived { get; set; }

        [InverseProperty(nameof(LogEntry.HardwareDevice))]
        public List<LogEntry> LogEntries { get; set; }
    }
}
