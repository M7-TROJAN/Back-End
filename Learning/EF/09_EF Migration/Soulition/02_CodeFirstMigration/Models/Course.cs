using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_CodeFirstMigration.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }

        // Navigation property
        public ICollection<Section> Sections { get; set; } = new List<Section>();
    }
}
