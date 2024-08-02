### Configuration for FakeTwitterV1

First, let's set up the EF Core models and `DbContext` for `FakeTwitterV1`.

```csharp
public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public ICollection<Tweet> Tweets { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

public class Tweet
{
    public int TweetId { get; set; }
    public int UserId { get; set; }
    public string TweetText { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public ICollection<Comment> Comments { get; set; }
}

public class Comment
{
    public int CommentId { get; set; }
    public int UserId { get; set; }
    public int TweetId { get; set; }
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public Tweet Tweet { get; set; }
}

public class FakeTwitterV1Context : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public FakeTwitterV1Context(DbContextOptions<FakeTwitterV1Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("tblUsers");
        modelBuilder.Entity<Tweet>().ToTable("tblTweets");
        modelBuilder.Entity<Comment>().ToTable("tblComments");
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Tweet>()
            .HasKey(t => t.TweetId);

        modelBuilder.Entity<Comment>()
            .HasKey(c => c.CommentId);

        modelBuilder.Entity<Tweet>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tweets)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Tweet)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TweetId);
    }
}
```

### Configuration for FakeTwitterV2

Now, let's set up the EF Core models and `DbContext` for `FakeTwitterV2`. The only change is the table names.

```csharp
public class FakeTwitterV2Context : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public FakeTwitterV2Context(DbContextOptions<FakeTwitterV2Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Tweet>().ToTable("Tweets");
        modelBuilder.Entity<Comment>().ToTable("Comments");
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.UserId);

        modelBuilder.Entity<Tweet>()
            .HasKey(t => t.TweetId);

        modelBuilder.Entity<Comment>()
            .HasKey(c => c.CommentId);

        modelBuilder.Entity<Tweet>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tweets)
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Tweet)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TweetId);
    }
}
```

### Adding Configuration in `Program.cs`

You need to configure the connection strings and set up the services in `Program.cs`.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();

        // Add FakeTwitterV1 context
        var v1ConnectionString = configuration.GetConnectionString("FakeTwitterV1");
        services.AddDbContext<FakeTwitterV1Context>(options => options.UseSqlServer(v1ConnectionString));

        // Add FakeTwitterV2 context
        var v2ConnectionString = configuration.GetConnectionString("FakeTwitterV2");
        services.AddDbContext<FakeTwitterV2Context>(options => options.UseSqlServer(v2ConnectionString));

        var serviceProvider = services.BuildServiceProvider();

        using (var v1Context = serviceProvider.GetService<FakeTwitterV1Context>())
        {
            var users = v1Context.Users.ToList();
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Username}");
            }
        }

        using (var v2Context = serviceProvider.GetService<FakeTwitterV2Context>())
        {
            var users = v2Context.Users.ToList();
            foreach (var user in users)
            {
                Console.WriteLine($"User: {user.Username}");
            }
        }
    }
}
```

### Configuration in `appsettings.json`

Make sure your `appsettings.json` file contains the connection strings for both databases.

```json
{
  "ConnectionStrings": {
    "FakeTwitterV1": "Server=your_server;Database=FakeTwitterV1;User Id=your_user;Password=your_password;",
    "FakeTwitterV2": "Server=your_server;Database=FakeTwitterV2;User Id=your_user;Password=your_password;"
  }
}
```

With this setup, you can use EF Core to work with both `FakeTwitterV1` and `FakeTwitterV2` databases, customizing your `DbContext` configurations accordingly. 

This example demonstrates the flexibility and power of EF Core's configuration options, allowing you to map your C# models to database tables efficiently.