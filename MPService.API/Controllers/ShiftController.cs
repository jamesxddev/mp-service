using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPService.Application.Shifts;
using MPService.Application.Shifts.DTOs;

namespace MPService.API.Controllers
{
    [Authorize]
    [Route("api/shift")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftAppService _shiftAppService;
        public ShiftController(IShiftAppService shiftAppService)
        {
            _shiftAppService = shiftAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift(ShiftRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Shift request cannot be null." });
            }
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            var result = await _shiftAppService.AddShiftAsync(request);
            return Ok(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetShifts(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { message = "Username is required." });
            }

            var result = await _shiftAppService.GetShiftsNotPaidAsync(username);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShift(Guid id, ShiftRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Shift request cannot be null." });
            }

            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "User ID is required." });
            }

            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Shift ID is required." });
            }

            var result = await _shiftAppService.UpdateShiftAsync(id, request);
            return Ok(result);
        }

    }
}
