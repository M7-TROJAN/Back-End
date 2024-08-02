using System.ComponentModel.DataAnnotations.Schema;

namespace _03_OverrideConfigurationUsingFluentAPI.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
