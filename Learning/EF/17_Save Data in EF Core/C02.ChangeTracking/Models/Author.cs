
using System.ComponentModel.DataAnnotations.Schema;

namespace C02.ChangeTracking.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        [NotMapped] // not to be mapped to the database
        public string FullName => $"{FName} {LName}";
        public List<Book> Books { get; set; } = new(); // navigation property
    }
}
