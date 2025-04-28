using MPService.Common.Entities;
using MPService.Domain.Shifts;
using System.ComponentModel.DataAnnotations;

namespace MPService.Domain.Users
{
    public class User : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(4)]
        public string EmployeeNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string MiddleName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        public DateTime Birthday { get; set; }

        public bool IsVerified { get; set; }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }


        public ICollection<Shift> Shift { get; set; } = [];
    }

}
