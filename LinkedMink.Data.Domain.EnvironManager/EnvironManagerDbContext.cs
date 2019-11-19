using LinkedMink.Base.Extensions;
using Microsoft.EntityFrameworkCore;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Domain.EnvironManager.Entities;

namespace LinkedMink.Data.Domain.EnvironManager
{
    public abstract class EnvironManagerDbContext : BaseDbContext<ClientUser>
    {
        static EnvironManagerDbContext()
        {
            DbSetPropertyByEntity[typeof(HardwareDevice)] = TypeHelpers.GetProperty<EnvironManagerDbContext>(t => t.HardwareDevices);
            DbSetPropertyByEntity[typeof(LogEntry)] = TypeHelpers.GetProperty<EnvironManagerDbContext>(t => t.LogEntries);
        }

        protected EnvironManagerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<HardwareDevice> HardwareDevices { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HardwareDevice>()
                .HasIndex(e => e.Host);
            /*
modelBuilder.Entity<RtsUserAddress>()
    .HasKey(e => new { e.RtsUserId, e.AddressId });

modelBuilder.Entity<RtsUserAddress>()
    .HasOne(ua => ua.RtsUser)
    .WithMany(u => u.RtsUserAddresses)
    .HasForeignKey(ua => ua.RtsUserId);

modelBuilder.Entity<RtsUserAddress>()
    .HasOne(ua => ua.Address)
    .WithMany(a => a.RtsUserAddresses)
    .HasForeignKey(ua => ua.AddressId);

modelBuilder.Entity<Tag>()
    .HasIndex(e => e.Name);

modelBuilder.Entity<LetterTag>()
    .HasKey(e => new { e.LetterId, e.TagId });

modelBuilder.Entity<LetterTag>()
    .HasOne(lt => lt.Letter)
    .WithMany(l => l.LetterTags)
    .HasForeignKey(lt => lt.LetterId);

modelBuilder.Entity<LetterTag>()
    .HasOne(lt => lt.Tag)
    .WithMany(t => t.LetterTags)
    .HasForeignKey(lt => lt.TagId);

modelBuilder.Entity<LetterRecipient>()
    .HasKey(e => new { e.LetterId, e.RecipientId });

modelBuilder.Entity<LetterRecipient>()
    .HasOne(er => er.Letter)
    .WithMany(e => e.LetterRecipients)
    .HasForeignKey(er => er.LetterId);

modelBuilder.Entity<LetterRecipient>()
    .HasOne(er => er.Recipient)
    .WithMany(t => t.LetterRecipients)
    .HasForeignKey(er => er.RecipientId);*/
        }
    }
}
