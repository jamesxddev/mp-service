using MPService.Application.Shifts.DTOs;
using MPService.Common;

namespace MPService.Application.Shifts
{
    public interface IShiftAppService
    {
        Task<Result<ShiftDto>> AddShiftAsync(ShiftRequest shiftRequest);
        Task<IEnumerable<ShiftDto>> GetShiftsByUsernameAsync(string username);
        Task<Result<ShiftDto>> UpdateShiftAsync(Guid id, ShiftRequest shiftRequest);
    }
}
