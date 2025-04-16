using Microsoft.EntityFrameworkCore;
using MPService.Domain.Users;

namespace MPService.Infrastructure.Persistence.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
