using System;
using System.ComponentModel.DataAnnotations.Schema;
using LinkedMink.Data.Base.Entities;

namespace LinkedMink.Data.Domain.EnvironManager.Entities
{
    public class LogEntry : BaseEntity
    {
        [Column(TypeName = "decimal(4, 1)")]
        public decimal RelativeHumidity { get; set; }
        [Column(TypeName = "decimal(4, 1)")]
        public decimal Temperature { get; set; }
        public DateTime RecordedOn { get; set; }

        public long HardwareDeviceId { get; set; }
        public HardwareDevice HardwareDevice { get; set; }
    }
}
