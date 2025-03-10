using Library.Data.Models.BaseModels;

namespace Library.Data.Models
{
    public class Patron : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Borrow> BorrowingRecords { get; set; } = new List<Borrow>();

    }
}
