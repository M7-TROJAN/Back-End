
ال **Mapster** هي مكتبة خفيفة وسريعة لتحويل الكائنات (Object Mapping) من نوع إلى نوع آخر في .NET.

بنستخدمها بشكل أساسي لتحويل:

* من **Entity → DTO**
* من **DTO → Entity**
* أو أي نوع آخر → لأي نوع تاني

مثلاً:

```csharp
User → UserDto
CreateUserRequest → User
```

---

## مميزات Mapster:

* أسرع من AutoMapper من حيث الأداء.
* لا تحتاج ملفات Configuration ضخمة.
* تدعم **mapping بالـ attributes** أو **بالكود**.
* تدعم both **manual mapping** و **projection** للـ LINQ.
* تقدر تـmap Lists و Nested Objects بسهولة.

---

## تثبيت Mapster:

```bash
dotnet add package Mapster
dotnet add package Mapster.DependencyInjection
```

---

## استخدام بسيط:

### 1. نفترض إن عندك:

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

```csharp
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

### 2. تستخدم Mapster كالتالي:

```csharp
var user = new User { Id = 1, Name = "Mahmoud" };

UserDto dto = user.Adapt<UserDto>(); // ready to use
```

---

## Mapping داخل الـ Controller أو Service:

```csharp
[HttpPost]
public IActionResult Create([FromBody] CreateUserRequest request)
{
    var user = request.Adapt<User>(); // Map from DTO to Entity

    _dbContext.Users.Add(user);
    _dbContext.SaveChanges();

    var userDto = user.Adapt<UserDto>(); // Map from Entity to DTO
    return Ok(userDto);
}
```

---

## استخدام Mapster مع Dependency Injection (Scoped Mapping)

### 1. في `Program.cs`:

```csharp
builder.Services.AddMapster();
```

### 2. تقدر تستخدم الـ `IMapper` Interface في أي مكان:

```csharp
public class UserService
{
    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserDto GetUserDto(User user)
    {
        return _mapper.Map<UserDto>(user);
    }
}
```

---

## إعدادات متقدمة (Configurations)

### 1. Basic Configuration

```csharp
TypeAdapterConfig<User, UserDto>.NewConfig()
    .Map(dest => dest.Name, src => src.Name.ToUpper());
```

### 2. Ignore Property

```csharp
TypeAdapterConfig<User, UserDto>.NewConfig()
    .Ignore(dest => dest.Name);
```

### 3. Global Configuration

```csharp
TypeAdapterConfig.GlobalSettings
    .NewConfig<User, UserDto>()
    .Map(dest => dest.Name, src => src.Name.ToUpper());
```

---

## Nested Mapping

```csharp
public class Order
{
    public int Id { get; set; }
    public User Customer { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
}
```

```csharp
TypeAdapterConfig<Order, OrderDto>.NewConfig()
    .Map(dest => dest.CustomerName, src => src.Customer.Name);
```

---

## Collections Mapping

```csharp
List<User> users = dbContext.Users.ToList();
List<UserDto> dtos = users.Adapt<List<UserDto>>();
```

---

## LINQ Projection

لو بتتعامل مع EF أو LINQ وتحب تعمل projection داخل الكويري:

```csharp
var dtos = dbContext.Users
    .ProjectToType<UserDto>() // 🚀 Mapster magic here
    .ToList();
```

---

## الفرق بين Adapt و AdaptToType

| الطريقة                      | الاستخدام                              |
| ---------------------------- | -------------------------------------- |
| `Adapt<T>()`                 | من كائن موجود إلى كائن جديد من النوع T |
| `Adapt(source, destination)` | تحديث كائن موجود بالقيم الجديدة        |

---

## استخدام Attributes بدلاً من Configuration

```csharp
[AdaptTo("[TargetType]")]
public class User { ... }
```

> مش منتشرة أوي بس بتكون مفيدة لو حابب تخلي المابنج داخل الكلاس نفسه.

---

## Create Custom Mapper Profile (في مشاريع كبيرة)

```csharp
public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
              .Map(dest => dest.Name, src => src.Name.ToUpper());
    }
}
```

وسجلها تلقائيًا:

```csharp
builder.Services.AddMapster(typeof(MappingConfigurations).Assembly);
```

---

## 📦 Summary

| Feature                      | مدعوم؟ |
| ---------------------------- | ------ |
| Mapping بين الكائنات         | ✅      |
| Mapping مع LINQ (Projection) | ✅      |
| Mapping مع DI                | ✅      |
| Ignore / Rename / Custom Map | ✅      |
| Mapping لـ Collections       | ✅      |
| Mapping لـ Nested Types      | ✅      |
| Auto discovery لـ profiles   | ✅      |

---
