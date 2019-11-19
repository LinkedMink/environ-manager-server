using System;

namespace LinkedMink.Data.Base.Entities
{
    public abstract class TrackedEntity : BaseEntity
    {
        public string ModifiedBy { get; protected set; }
        public DateTime ModifiedDateTime { get; protected set; }
        public string CreatedBy { get; protected set; }
        public DateTime CreatedDateTime { get; protected set; }

        public void SetCreatedBy(string user)
        {
            CreatedBy = user;
            ModifiedDateTime = DateTime.Now;
            ModifiedBy = user;
            ModifiedDateTime = DateTime.Now;
        }

        public void UpdateModifiedBy(string user)
        {
            ModifiedBy = user;
            ModifiedDateTime = DateTime.Now;
        }
    }
}
