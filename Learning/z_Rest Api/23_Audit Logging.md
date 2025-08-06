##  **What is Audit Logging?**

Audit Logging is a common requirement in most applications. It allows us to track:

* **Who created a record**
* **When it was created**
* **Who modified it**
* **When it was modified**

This is essential for:

* Debugging
* Security
* Historical record
* Accountability

---

## ✅ **The Best Way to Implement It in EF Core: Inheritance**

You can define a base class `AuditableEntity` that contains common audit fields:

```csharp
public class AuditableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedById { get; set; } = string.Empty;

    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedById { get; set; }

    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }
}
```

Then inherit it from your entities:

```csharp
public sealed class Poll : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public DateOnly StartsAt { get; set; }
    public DateOnly EndsAt { get; set; }
}
```

---

## ⚠️ **Note Before Testing (IMPORTANT)**

If you want to test your implementation and re-insert records from scratch, you may need to reset the auto-increment identity of your table:

```sql
DELETE FROM Polls;
DBCC CHECKIDENT ('Polls', RESEED, 0);
```

### ⚠️ WARNING:

This will **delete all records** from the `Polls` table and **reset the ID to 1**. Do **NOT** run this on production.

---

## 🔧 **Assign Audit Values Automatically (Multiple Ways)**

There are multiple ways to automatically populate `CreatedAt`, `CreatedById`, `UpdatedAt`, and `UpdatedById`.

---

### ✅ **Option 1: Use EF Core Interceptors (Advanced)**

EF Core provides **interceptors** that allow you to intercept saving behavior and manipulate data.

This is more reusable but also more advanced.

Example: Create a class implementing `SaveChangesInterceptor`, then inject it into the `DbContext`.

---

### ✅ **Option 2: Use Triggers in the Database (SQL level)**

If you want to do audit logging **outside your app logic**, you can create database-level triggers to insert audit data on changes.

**Not recommended** unless you're working with legacy or external systems.

---

### ✅ **Option 3: Override `SaveChanges` / `SaveChangesAsync` in `DbContext`**

This is the most recommended and clean approach.

You inspect all entities that inherit from `AuditableEntity` and modify their properties before saving:

```csharp
public override int SaveChanges()
{
    var entries = ChangeTracker.Entries<AuditableEntity>();
    foreach (var entry in entries)
    {
        if (entry.State == EntityState.Added)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedById = "system"; // replace later
        }
        else if (entry.State == EntityState.Modified)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedById = "system"; // replace later
        }
    }
    return base.SaveChanges();
}
```

Same for the async version.

---

## 👤 **How to Get the Logged-In User’s ID**

To assign `CreatedById` and `UpdatedById`, you need access to the current user.

You can achieve this using:

### ✅ `IHttpContextAccessor`

This gives you access to the current HTTP context and allows you to read the `User` from the JWT token.

---

### 🛠 Injecting IHttpContextAccessor into `DbContext`

Here’s how your `DbContext` would look with this implemented:

```csharp
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<ApplicationUser>(options)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public DbSet<Poll> Polls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedById = currentUserId ?? "system";
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedById = currentUserId ?? "system";
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.CreatedById = currentUserId ?? "system";
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
                entry.Entity.UpdatedById = currentUserId ?? "system";
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
```

---

## ⚠️ Notes:

* `ClaimTypes.NameIdentifier` should match the claim name you're storing in the JWT (usually the `sub`).
* If the user is not authenticated, `CreatedById` will fall back to `"system"`.
* Make sure `IHttpContextAccessor` is registered in `Program.cs`:

```csharp
builder.Services.AddHttpContextAccessor();
```

---

## 🧪 Test Tip

When writing integration tests or using background services (like workers), `HttpContext` may be null. Handle that gracefully.

---

## ✅ Summary

| Part                   | Purpose                                      |
| ---------------------- | -------------------------------------------- |
| `AuditableEntity`      | Base class to hold audit fields              |
| `SaveChanges override` | Automatically populate audit values          |
| `IHttpContextAccessor` | Get current user ID from the request context |
| `DbContext injection`  | Use the user ID while saving entities        |
| `SQL Reseed Command`   | Reset identity column during dev/testing     |
