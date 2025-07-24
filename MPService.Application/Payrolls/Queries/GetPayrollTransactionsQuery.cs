using MediatR;
using MPService.Application.Payrolls.DTOs;
using MPService.Domain.Payrolls;

namespace MPService.Application.Payrolls.Queries
{
    public record GetPayrollTransactionsQuery(string username, bool isPaid, DateTime startDate, DateTime endDate) 
        : IRequest<PayrollViewDto>;

}
