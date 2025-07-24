using MediatR;
using MPService.Application.Payrolls.DTOs;
using MPService.Application.Shifts.DTOs;
using MPService.Application.Users.DTOs;
using MPService.Common;
using MPService.Common.Exceptions;
using MPService.Domain.Payrolls;
using MPService.Domain.Shifts;
using MPService.Domain.Users;

namespace MPService.Application.Payrolls.Queries
{
    public class GetPayrollTransactionsQueryHandler : IRequestHandler<GetPayrollTransactionsQuery, PayrollViewDto>    
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IUserRepository _userRepository;

        public GetPayrollTransactionsQueryHandler(IShiftRepository shiftRepository, IUserRepository userRepository)
        {
            _shiftRepository = shiftRepository;
            _userRepository = userRepository;
        }

        public async Task<PayrollViewDto> Handle(GetPayrollTransactionsQuery request, CancellationToken cancellationToken)
        {
            var result = new PayrollViewDto();

            var user = await _userRepository.GetByUsernameAsync(request.username);
            if (user == null)
            {
                throw new NotFoundException("User", request.username);
            }

            result.Username = request.username;
            var shiftResult = await _shiftRepository
                .GetShiftByUsernameAndDateRangeAsync(request.username, request.isPaid, request.startDate, request.endDate);

            if (shiftResult == null || !shiftResult.Any())
            {
                return result;
            }

            var shifts = shiftResult.Select(x => new ShiftDto 
            {
                ShiftId = x.Id.ToString(),
                TimeIn = x.TimeIn,
                TimeOut = x.TimeOut,
            }).ToList();

            result.Shifts = shifts;

            return result;
        }
    }
}
