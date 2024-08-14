
namespace C01.BasicSaveWithTracking.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; } // foreign key
        public Author Author { get; set; } // navigation property
    }
}
