using Microsoft.EntityFrameworkCore;
using MPService.Common;
using MPService.Domain.Shifts;

namespace MPService.Infrastructure.Persistence.Shifts
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly AppDbContext _dbContext;
        public ShiftRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Shift?> GetShiftByIdAsync(Guid id)
        {
            return await _dbContext.Shift
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> AddShiftAsync(Shift shift)
        {
            _dbContext.Shift.Add(shift);

            return await SaveChangesAsync();

        }

        public async Task<bool> UpdateShiftAsync(Shift shift)
        {
            _dbContext.Shift.Update(shift);
            
            return await SaveChangesAsync();
        }

        public async Task<IEnumerable<Shift>> GetShiftByUsernameAsync(string username)
        {
            var shifts = await _dbContext.Shift
                .AsNoTracking()
                .Where(x => x.User != null && x.User.Username == username)
                .OrderByDescending(x => x.TimeIn)
                .ToListAsync();

            return shifts;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
