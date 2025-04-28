using Microsoft.EntityFrameworkCore;
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
            });

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.Property(e => e.TimeIn).IsRequired();

                entity.HasOne(a => a.User)
                .WithMany(s => s.Shift)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            });

        }

    }
}
