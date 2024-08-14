
namespace C02.ChangeTracking.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; } // foreign key
        public Author Author { get; set; } // navigation property
    }
}
