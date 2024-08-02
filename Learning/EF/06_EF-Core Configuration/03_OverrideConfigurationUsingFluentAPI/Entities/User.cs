using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _03_OverrideConfigurationUsingFluentAPI.Entities
{
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
