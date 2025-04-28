using MPService.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace MPService.Domain.Shifts
{
    public class Shift
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaidDate { get; set; }

        public void CreateNewShift(Guid userId, DateTime timeIn) 
        {
            Id = Guid.NewGuid();
            UserId = userId;
            TimeIn = timeIn;
        }

        public void UpdateShift(DateTime timeOut)
        {
            TimeOut = timeOut;   
        }

        public User? User { get; set; }
    }
}
