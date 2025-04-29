namespace MPService.Application.Shifts.DTOs
{
    public class ShiftAttendanceDto
    {
        public bool PresentToday { get; set; }
        public bool ShiftEnded { get; set; }
        public string ShiftId { get; set; } = string.Empty;
        public List<ShiftDto> Shifts { get; set; } = [];
    }
}
