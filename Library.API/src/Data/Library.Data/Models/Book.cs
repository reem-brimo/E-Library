using Library.Data.Models;
using Library.Data.Models.BaseModels;
using Library.Data.Models.Security;

namespace Library.Data.Models
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public string ISBN { get; set; }
        public ICollection<Borrow> BorrowingRecords { get; set; } = new List<Borrow>();

    }
}
