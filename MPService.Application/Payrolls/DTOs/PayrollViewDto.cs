using MPService.Application.Shifts.DTOs;

namespace MPService.Application.Payrolls.DTOs
{
    public class PayrollViewDto
    {
        public string Username { get; set; } = string.Empty;

        public List<ShiftDto> Shifts { get; set; } = new List<ShiftDto>();
    }
}
