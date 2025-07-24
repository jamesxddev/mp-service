using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MPService.Application.Payrolls.DTOs;
using MPService.Application.Payrolls.Queries;
using MPService.Common.Exceptions;

namespace MPService.API.Controllers
{
    [Authorize]
    [Route("api/payroll")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PayrollController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("view/{username}")]
        public async Task<IActionResult> GetPayrollView(string username, [FromQuery] DateTime startDate, DateTime endDate) 
        {
            var result = await _mediator.Send(new GetPayrollTransactionsQuery(username, false, startDate, endDate));
            return Ok(result);
        }




    }
}
