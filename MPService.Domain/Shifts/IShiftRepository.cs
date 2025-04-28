namespace MPService.Domain.Shifts
{
    public interface IShiftRepository
    {
        Task<Shift?> GetShiftByIdAsync(Guid id);
        Task<bool> AddShiftAsync(Shift shift);
        Task<bool> UpdateShiftAsync(Shift shift);
        Task<IEnumerable<Shift>> GetShiftByUsernameAsync(string username);


    }
}
