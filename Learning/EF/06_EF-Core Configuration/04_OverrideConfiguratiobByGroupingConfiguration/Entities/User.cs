using System.ComponentModel.DataAnnotations;

namespace _04_OverrideConfiguratiobByGroupingConfiguration.Entities
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
