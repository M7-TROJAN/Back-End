using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _02_OverrideConfigurationByDataAnnotations.Entities
{
    [Table("tblUsers")] // This attribute is used to specify the table name in the database that will be used for this entity
    public class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }

        override public string ToString()
        {
            return $"[{UserId}] - {Username}";
        }

    }
}
