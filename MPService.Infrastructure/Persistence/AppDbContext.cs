using Microsoft.EntityFrameworkCore;
using MPService.Domain.Payrolls;
using MPService.Domain.Shifts;
using MPService.Domain.Users;

namespace MPService.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Shift> Shift { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(i => i.Username)
                .IsUnique();

                entity.HasIndex(i => i.Email)
                .IsUnique();

                entity.HasMany(s => s.Shift)
                .WithOne(a => a.User)
                .HasForeignKey(x => x.UserId);

                entity.HasMany(e => e.PayrollTransactions)
                .WithOne(a => a.User)
                .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.TimeIn).IsRequired();

                entity.HasOne(a => a.User)
                .WithMany(s => s.Shift)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<PayrollTransaction>(entity => 
            {
                entity.ToTable("PayrollTransactions");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.PayrollDate)
                .IsRequired();

                entity.Property(e => e.TotalDays)
                .IsRequired();

                entity.Property(e => e.TotalPay)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

                entity.Property(e => e.DateCreated)
                .IsRequired();

                entity.HasOne(a => a.User)
                .WithMany(s => s.PayrollTransactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            });

        }

    }
}
