﻿using System.ComponentModel.DataAnnotations.Schema;

namespace _03_OverrideConfigurationUsingFluentAPI.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string TweetText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
