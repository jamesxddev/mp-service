using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MPService.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplicationDbContext(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            if (configuration == null)
            {
                throw new InvalidOperationException("IConfiguration service is not registered.");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                var conn = configuration.GetConnectionString("DefaultConnection");

                options.UseSqlServer(conn, sql => sql
                    .MigrationsAssembly("MPService.Infrastructure")
                    .EnableRetryOnFailure());
            });

        }
    }
}
