using MPService.Domain.Users;

namespace MPService.Domain.Payrolls
{
    public class PayrollTransaction
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime PayrollDate { get; set; }
        public int TotalDays { get; set; }
        public decimal TotalPay { get; set; }
        public DateTime DateCreated { get; set; }

        public User? User { get; set; }

    }
}
