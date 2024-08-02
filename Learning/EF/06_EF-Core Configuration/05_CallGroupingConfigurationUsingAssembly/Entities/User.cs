using System.ComponentModel.DataAnnotations;

namespace _05_CallGroupingConfigurationUsingAssembly.Entities
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
