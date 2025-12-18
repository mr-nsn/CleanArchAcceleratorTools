# CleanArchAcceleratorTools.Infrastructure

This module is part of `CleanArchAcceleratorTools`, a toolkit to accelerate building applications with Clean Architecture. It implements the Infrastructure layer.

Related modules:
- [CleanArchAcceleratorTools.Controller](../CleanArchAcceleratorTools.Controller)
- [CleanArchAcceleratorTools.Application](../CleanArchAcceleratorTools.Application)
- [CleanArchAcceleratorTools.Domain](../CleanArchAcceleratorTools.Domain)
- [CleanArchAcceleratorTools.AllInOne](../CleanArchAcceleratorTools.AllInOne)

📄 License: [MIT](./LICENSE)

---

## 🧠 Principles

### 🔗 Role in the architecture

- Controller: Exposes endpoints, it does not contain business rules.
- Application: Orchestrates use cases and references the Domain and repository.
- Domain: Pure domain with business rules and validations.
- Infrastructure: Implements persistence/mapping for Domain entities.
- Mediator: Handles commands, queries, and notifications.

---

## ✨ Features

- **Dynamic Selects** – Project typed objects based on property names.
- **Dynamic Filtering** – Filter queries with a fluent builder using clauses and operators.
- **Dynamic Sort** – Sort queries with multiple fields and directions.
- **Pagination Utilities** – Helpers for `IQueryable` including filtering and ordering.
- **Parallel Query Execution** – Process large query sets in parallel.
- **Simplified Entity Mapping** – Configure and register mappings for domain models.
- **Generic Repository** – Reusable repository pattern for CRUD and advanced queries.
- **DataTable Creation** – Build `DataTable` from lists for procedures and raw SQL.
- **Extensible & Lightweight** – Modular, discoverable, and easy to integrate.

---

## 🧩 Compatibility

Multi-target:
- .NET 6
- .NET 7
- .NET 8
- .NET 9

Use an EF Core version compatible with your target framework.

---

## ✅ Prerequisites

- [EF Core 6+](https://www.nuget.org/packages/FluentValidation) (match your framework)
- A configured `DbContext`
- Optional: `Newtonsoft.Json` or `System.Text.Json` for serialization scenarios

---

### 🚀 Installation
``` bash
dotnet add package CleanArchAcceleratorTools.Infrastructure --version x.x.x
```

## ⚡ Quickstart

1. If you have separated Application project add this module reference on it. Add allInOne if using a single project.
2. Register your `DbContext` in DI.
    ```csharp
    builder.Services.AddDbContext<YourContext>();
    builder.Services.AddDbContextFactory<YourContext, YourContextFactory>();
    ```
3. Configure the generic DbContext DI and CleaArch services
    ```csharp
    builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<YourContext>());
    builder.Services.AddScoped<IDbContextFactory<DbContext>>(sp => new DbContextFactoryAdapter(sp.GetRequiredService<IDbContextFactory<YourContext>>()));

    builder.Services.AddCleanArchConfiguration<YourContext>();
    ```
4. Create your models as needed. Needed to Inherit from a base `Entity` if you want to use common properties.
5. (Optional) Map your entities using `EntityTypeConfiguration<T>` as the usage example below.
6. (Optional) Create your repositories extending `IGenericRepository<T>` and `GenericRepository<T>` as example below.
7. Use the features provided.
---

## 🛠️ Usage Examples

### Dynamic Select

Tired of `Include` and `ThenInclude` everything? With `DynamicSelect`, you can easily project only the properties you need, without the overhead of loading entire entities and without having to make the `Select` manually each time.
You can simply add the fields you want for each relationship and the library will handle the rest. It supports simple properties and nested complex relationships, including Collections.

It includes filtering and ordering capabilities too.

Bonus Tip: Enable `NullValueHandling = NullValueHandling.Ignore` in `Newtonsoft.Json` serialization or similars to have a GraphQL like solution for your REST APIs.

#### You can select simple properties of the entity
```csharp

var courses = await _yourContext.Courses
    .DynamicSelect(nameof(Course.Id), nameof(Course.Title))
    .ToListAsync();
```

#### Or you can select complex properties, including collections
```csharp
string[] AllRelationships =
[
    string.Format("{0}", nameof(Course.Id)),
    string.Format("{0}", nameof(Course.InstructorId)),
    string.Format("{0}", nameof(Course.Title)),
    string.Format("{0}", nameof(Course.CreatedAt)),
    string.Format("{0}.{1}", nameof(Course.Instructor), nameof(Course.Instructor.Id)),
    string.Format("{0}.{1}", nameof(Course.Instructor), nameof(Course.Instructor.FullName)),
    string.Format("{0}.{1}", nameof(Course.Instructor), nameof(Course.Instructor.CreatedAt)),
    string.Format("{0}.{1}.{2}", nameof(Course.Instructor), nameof(Instructor.Profile), nameof(Profile.Id)),
    string.Format("{0}.{1}.{2}", nameof(Course.Instructor), nameof(Instructor.Profile), nameof(Profile.InstructorId)),
    string.Format("{0}.{1}.{2}", nameof(Course.Instructor), nameof(Instructor.Profile), nameof(Profile.Bio)),
    string.Format("{0}.{1}.{2}", nameof(Course.Instructor), nameof(Instructor.Profile), nameof(Profile.LinkedInUrl)),
    string.Format("{0}.{1}.{2}", nameof(Course.Instructor), nameof(Instructor.Profile), nameof(Profile.CreatedAt)),
    string.Format("{0}.{1}", nameof(Course.Modules), nameof(Module.Id)),
    string.Format("{0}.{1}", nameof(Course.Modules), nameof(Module.CourseId)),
    string.Format("{0}.{1}", nameof(Course.Modules), nameof(Module.Name)),
    string.Format("{0}.{1}", nameof(Course.Modules), nameof(Module.CreatedAt)),
    string.Format("{0}.{1}.{2}", nameof(Course.Modules), nameof(Module.Lessons), nameof(Lesson.Id)),
    string.Format("{0}.{1}.{2}", nameof(Course.Modules), nameof(Module.Lessons), nameof(Lesson.ModuleId)),
    string.Format("{0}.{1}.{2}", nameof(Course.Modules), nameof(Module.Lessons), nameof(Lesson.Title)),
    string.Format("{0}.{1}.{2}", nameof(Course.Modules), nameof(Module.Lessons), nameof(Lesson.Duration)),
    string.Format("{0}.{1}.{2}", nameof(Course.Modules), nameof(Module.Lessons), nameof(Lesson.CreatedAt))
];

var courses = await _yourContext.Courses
    .DynamicSelect(AllRelationships)
    .ToListAsync();
```

##### You can filter and sort as well
```csharp
var dynamicFilter = ...;
var dynamicSort = ...;

// populate dynamicFilter and dynamicSort with Set properties or Builder...

var courses = await _yourContext.Courses            
    .DynamicSelect(nameof(Course.Id), nameof(Course.Title))
    .Where(dynamicFilter.CompileFilter())
    .OrderBy(dynamicSort.CompileSort().First().Expression)
    .ToListAsync();
```

### Pagination

Effortlessly paginate your queries, with dynamic filtering and dynamic ordering capabilities:
```csharp
var dynamicFilter = ...;
var dynamicSort = ...;

// populate dynamicFilter and dynamicSort with Set properties or Builder...

var queryFilter = new QueryFilterBuilder<Course>()
    .WithPage(1)
    .WithPageSize(100)
    .WithDynamicFilter(dynamicFilter)
    .WithDynamicSort(dynamicSort)
    .WithFields(SelectsDefaults<Course>.BasicFields)
    .Build();

var firstPage = await _yourContext.Courses
    .OrderBy(x => x.Id)
    .GetPagination(queryFilter)
    .ToPaginationResultListAsync();
```

### Entity Mapping

Map your entities with ease:
```csharp
public class CourseMap : EntityTypeConfiguration<Course>
{
    public override void Map(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("TB_COURSE");

        builder.HasKey(x => x.Id)
               .HasName("SQ_COURSE");

        builder.Property(x => x.Id)
               .HasColumnName("SQ_COURSE")
               .HasColumnType("bigint")
               .UseIdentityColumn();

        builder.Property(x => x.InstructorId)
               .HasColumnName("SQ_INSTRUCTOR")
               .HasColumnType("bigint");

        builder.Property(x => x.Title)
               .HasColumnName("TX_TITLE")
               .HasColumnType("nvarchar(100)");

        builder.Property(x => x.CreatedAt)
               .HasColumnName("DT_CREATION")
               .HasColumnType("nvarchar(100)");
    }
}

...

public static class MappingsHolder
{
    public static Dictionary<Type, IEntityTypeConfiguration> GetMappings()
    {
        var mappings = new Dictionary<Type, IEntityTypeConfiguration>();

        mappings.Add(typeof(Course), new CourseMap());

        return mappings;
    }
}

...

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.RegisterModelsMapping(MappingsHolder.GetMappings());
}
```

### Parallel Query Execution
Execute queries in parallel to improve performance on large datasets:
```csharp
await ParallelQueryExecutor.DoItParallelAsync
(
    () => _registeredContextFactory.Create(Guid.NewGuid().ToString()).Courses.OrderBy(x => x.Id).AsQueryable(),
    new ParallelParams
    {
        TotalRegisters = _yourContext.Courses.Count(),
        BatchSize = 10,
        MaximumDegreeOfParalelism = Environment.ProcessorCount,
        MaxDegreeOfProcessesPerThread = 1
    },
    _logger
);
```

### Generic Repository

Leverage a robust, reusable repository for your entities:
```csharp
public interface ICourseRepository : IGenericRepository<Course> { }

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    private readonly YourContext _yourContext;
    private readonly IDbContextRegistratorService<YourContext> _registeredContextFactory;
    private readonly IApplicationLogger _logger;

    public CourseRepository(
        YourContext context,
        IDbContextRegistratorService<YourContext> registeredContextFactory,
        IApplicationLogger logger
    ) : base(context, new DbContextRegistratorServiceAdapter(registeredContextFactory))
    {
        _yourContext = context;
        _registeredContextFactory = registeredContextFactory;
        _logger = logger;
    }
}

...

// Usage
var allCourses = await _courseRepository.GetAllAsync();
```

### DataTable Creation
Easily create DataTables, from Lists, for use in stored procedures and raw SQL queries:
```csharp
var columnsOrder = new Dictionary<string, int>()
{
    { "Id", 0 },
    { "InstructorId", 1 },
    { "Title", 2 }
};
var courses = await _courseRepository.GetAllAsync();
var dataTable = courses.ToDataTable(columnsOrder, context);
```

#### Obs: The list type must be mapped in the context!

---

## 📚 Example Project

See the - [CleanArchAcceleratorTools.Examples](../../examples/CleanArchAcceleratorTools.Examples) project for a complete working example, including setup and advanced scenarios. It implements a fully working API with all other modules implemented.

---

## 🧠 Tips

- Prefer `DynamicSelect` to reduce materialization and network payloads.
- Tune `ParallelParams` (`BatchSize`, `MaximumDegreeOfParalelism`) per environment and workload.
- Keep entity maps consistent and centralized via `MappingsHolder` for discoverability.
- Centralize selection fields in a constant for reuse as in [CourseSelects](<../../examples/CleanArchAcceleratorTools.Examples/3 - Domain/Aggregates/Courses/Selects/CourseSelects.cs>).

---

## 🤝 Contributing

Contributions are welcome! Open issues or submit PRs for features, bug fixes, or documentation improvements.
Contributing guidelines are under construction.
