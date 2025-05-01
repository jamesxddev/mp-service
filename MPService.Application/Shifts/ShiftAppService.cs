using MPService.Application.Shifts.DTOs;
using MPService.Application.Users.DTOs;
using MPService.Common;
using MPService.Domain.Shifts;
using MPService.Domain.Users;

namespace MPService.Application.Shifts
{
    public class ShiftAppService : IShiftAppService
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IUserRepository _userRepository;
        public ShiftAppService(IShiftRepository shiftRepository, IUserRepository userRepository)
        {
            _shiftRepository = shiftRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<ShiftDto>> AddShiftAsync(ShiftRequest shiftRequest)
        {
            var user = await _userRepository.GetByUsernameAsync(shiftRequest.Username);
            if (user == null)
            {
                return Result<ShiftDto>.Failure("User not found.");
            }

            var shift = new Shift();
            shift.CreateNewShift(user.Id, DateTime.Now);

            var result = await _shiftRepository.AddShiftAsync(shift);

            if (result)
            {
                var shiftDto = new ShiftDto
                {
                    ShiftId = shift.Id.ToString(),
                    TimeIn = shift.TimeIn,
                    TimeOut = shift.TimeOut,
                };
                return Result<ShiftDto>.Success(shiftDto);

            }
            else
            {
                return Result<ShiftDto>.Failure("Failed to add shift.");
            }

        }

        public async Task<IEnumerable<ShiftDto>> GetShiftsByUsernameAsync(string username)
        {
            var shiftDtos = new List<ShiftDto>();

            var shifts = await _shiftRepository.GetShiftByUsernameAsync(username);
            shiftDtos.AddRange(shifts.Select(s => new ShiftDto
            {
                ShiftId = s.Id.ToString(),
                TimeIn = s.TimeIn,
                TimeOut = s.TimeOut,
            }));

            return shiftDtos;
        }

        public async Task<ShiftAttendanceDto> GetShiftsNotPaidAsync(string username)
        {
            var shiftAttendanceDto = new ShiftAttendanceDto
            {
                PresentToday = false,
                Shifts = []
            };

            var shiftDtos = new List<ShiftDto>();

            var shifts = await _shiftRepository.GetShiftByUsernameAsync(username);

            var shiftsNotPaid = shifts
                .Where(x => !x.IsPaid)
                .Select(x => new ShiftDto 
                {
                    ShiftId = x.Id.ToString(),
                    TimeIn = x.TimeIn,
                    TimeOut = x.TimeOut != default ? x.TimeOut : null,
                });

            shiftDtos.AddRange(shiftsNotPaid);

            var shiftToday = shiftDtos.FirstOrDefault(s => s.TimeIn.Date == DateTime.Now.Date);

            if (shiftToday != null)
            {
                shiftAttendanceDto.ShiftId = shiftToday.ShiftId;
                var presentToday = shiftToday.TimeIn != default;
                var shiftEnded = shiftToday.TimeOut != default;

                shiftAttendanceDto.PresentToday = presentToday;
                shiftAttendanceDto.ShiftEnded = shiftEnded;
            }

            shiftAttendanceDto.Shifts = shiftDtos;

            return shiftAttendanceDto;
        }

        public async Task<Result<ShiftDto>> UpdateShiftAsync(Guid id, ShiftRequest shiftRequest)
        {
            var shift = await _shiftRepository.GetShiftByIdAsync(id);
            if (shift == null)
            {
                return Result<ShiftDto>.Failure("Shift not found.");
            }

            shift.TimeOut = DateTime.Now;

            var result = await _shiftRepository.UpdateShiftAsync(shift);

            if (result)
            {
                var shiftDto = new ShiftDto
                {
                    ShiftId = shift.Id.ToString(),
                    TimeIn = shift.TimeIn,
                    TimeOut = shift.TimeOut,
                };
                return Result<ShiftDto>.Success(shiftDto);
            }
            else
            {
                return Result<ShiftDto>.Failure("Failed to update shift.");
            }

        }
    }
}
