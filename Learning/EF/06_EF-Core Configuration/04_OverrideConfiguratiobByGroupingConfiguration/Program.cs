namespace _04_OverrideConfiguratiobByGroupingConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new Data.FakeTwitterV1Context())
            {
                Console.WriteLine("\n_________ Users __________\n");
                foreach (var user in context.Users)
                {
                    Console.WriteLine(user);
                }

                Console.WriteLine("\n_________ Tweets __________\n");
                foreach (var tweet in context.Tweets)
                {
                    Console.WriteLine(tweet.TweetText);
                }

                Console.WriteLine("\n_________ Comments __________\n");
                foreach (var comment in context.Comments)
                {
                    Console.WriteLine(comment.CommentText);
                }
            }

            Console.ReadKey();
        }
    }
}
