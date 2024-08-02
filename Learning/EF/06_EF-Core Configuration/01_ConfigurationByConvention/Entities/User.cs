using System.ComponentModel.DataAnnotations;

namespace _01_ConfigurationByConvention.Entities
{
    public class User
    {
        // Primary key convention => [Id, id , ID] , [{ClassName}Id]
        // if you want to use another name for the primary key you can use the [Key] attribute like this:
        //[Key]
        //public int number { get; set; }

        public int UserId { get; set; } // Primary key

        [Required] // Not null
        public string Username { get; set; }

        override public string ToString()
        {
            return $"[{UserId}] - {Username}";
        }

    }
}
