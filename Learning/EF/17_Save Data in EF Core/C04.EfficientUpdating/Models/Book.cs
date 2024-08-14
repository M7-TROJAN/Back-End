
namespace C04.EfficientUpdating.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; } // foreign key
        public Author Author { get; set; } // navigation property
        public decimal Price { get; set; }
    }
}
