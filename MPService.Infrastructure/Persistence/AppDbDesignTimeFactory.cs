using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MPService.Infrastructure.Persistence
{
    class AppDbDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseSqlServer("Server=JAMESPC;Database=MpDb-dev;Trusted_Connection=True;TrustServerCertificate=True;");
            return new AppDbContext(builder.Options);

        }
    }
}
