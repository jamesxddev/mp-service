namespace MPService.Application.Shifts.DTOs
{
    public class ShiftDto
    {
        public string ShiftId { get; set; } = string.Empty;
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }

    }
}
