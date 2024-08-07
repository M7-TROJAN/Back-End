using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_CodeFirstMigration.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string? FName { get; set; }

        public string? LName { get; set; }

        [RegularExpression("^[fm]$", ErrorMessage = "Gender must be 'f' or 'm'")]
        public char Gender { get; set; }

        public ICollection<Section> Sections { get; set; } = new List<Section>();

    }
}
