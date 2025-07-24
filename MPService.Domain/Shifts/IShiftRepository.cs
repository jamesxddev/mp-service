namespace MPService.Domain.Shifts
{
    public interface IShiftRepository
    {
        Task<Shift?> GetShiftByIdAsync(Guid id);
        Task<bool> AddShiftAsync(Shift shift);
        Task<bool> UpdateShiftAsync(Shift shift);
        Task<IEnumerable<Shift>> GetShiftByUsernameAsync(string username);
        Task<IEnumerable<Shift>> GetShiftByUsernameAndDateRangeAsync(string username, bool isPaid, DateTime startDate, DateTime endDate);


    }
}
