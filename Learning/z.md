
###  يعني إيه RESTful API؟

هو أي API معمول على مبادئ REST. بمعنى:

* كل حاجة ليها URI واضح.
* بنستخدم HTTP Methods (GET, POST, PUT, DELETE).
* البيانات غالبًا بتكون بصيغة JSON.
* مفيش جلسة (Stateless).
* في استجابة واضحة بحالة الـ Request (Status Code).

---

## 🎯 أولًا: يعني إيه Fluent API؟

* هو **طريقة لكتابة الـ Configuration الخاصة بالـ Entities** بدل ما تكتبها Data Annotations فوق الكلاس أو البروبريتز.
* بيتكتب كله جوه **`OnModelCreating(ModelBuilder builder)`** في الـ `DbContext`.
* اسمه Fluent لأنه بيعتمد على **method chaining** (تقدر تربط أكتر من إعداد ورا بعض).

---

## 🎯 ليه نستخدم Fluent API؟

1. لو عاوز تعمل Configurations مش مدعومة بالـ DataAnnotations.
2. لو عاوز فصل واضح بين الـ Domain classes والـ EF Configurations.
3. لو المشروع كبير، عادةً بنعمل Configurations منفصلة (EntityTypeConfiguration classes).

---

## 🎯 أهم الحاجات اللي ممكن يسألك عنها في الإنترفيو

### 1. Primary Key

```csharp
builder.Entity<User>()
    .HasKey(u => u.Id);
```

### 2. Column Configurations

```csharp
builder.Entity<User>()
    .Property(u => u.Email)
    .HasMaxLength(200)
    .IsRequired();
```

* `HasMaxLength(200)` → يحدد طول العمود.
* `IsRequired()` → العمود لا يقبل NULL.

---

### 3. Relationships (One-to-Many, Many-to-Many, One-to-One)

#### One-to-Many

```csharp
builder.Entity<Order>()
    .HasOne(o => o.User) // Each Order has one User
    .WithMany(u => u.Orders) // Each User has many Orders
    .HasForeignKey(o => o.UserId); // Foreign Key
```

#### One-to-One

```csharp
builder.Entity<User>()
    .HasOne(u => u.Profile)
    .WithOne(p => p.User)
    .HasForeignKey<Profile>(p => p.UserId);
```

#### Many-to-Many

(في EF Core 5+ بقى supported بشكل مباشر)

```csharp
builder.Entity<Student>()
    .HasMany(s => s.Courses)
    .WithMany(c => c.Students)
    .UsingEntity(j => j.ToTable("StudentCourses"));
```

---

### 4. Table Configurations

```csharp
builder.Entity<User>()
    .ToTable("Users"); // اسم الجدول
```

---

### 5. Indexes

```csharp
builder.Entity<User>()
    .HasIndex(u => u.Email)
    .IsUnique();
```

---

### 6. Default Values

```csharp
builder.Entity<User>()
    .Property(u => u.CreatedAt)
    .HasDefaultValueSql("GETDATE()");
```

---

### 7. Ignore Property

```csharp
builder.Entity<User>()
    .Ignore(u => u.TempData);
```

---

### 8. Composite Key

```csharp
builder.Entity<OrderProduct>()
    .HasKey(op => new { op.OrderId, op.ProductId });
```

---


* "إيه هو Fluent API؟" → تقوله هو أسلوب لعمل Configuration للـ Entities باستخدام كود جوه الـ DbContext بدل الـ DataAnnotations، وبيديك تحكم أكتر.
* "طب لو عاوز تعمل شرط كذا؟" → تبدأ تديه مثال زي اللي فوق.
