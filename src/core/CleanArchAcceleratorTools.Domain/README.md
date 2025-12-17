# CleanArchAcceleratorTools.Domain

This module is part of `CleanArchAcceleratorTools`, a toolkit to accelerate building applications with Clean Architecture. The Domain module contains the core model and business rules. It is persistence and framework agnostic: focus on Entities, Aggregates, Domain Services, Validations, and domain messages.

Related modules:
- [CleanArchAcceleratorTools.Controller](../CleanArchAcceleratorTools.Controller)
- [CleanArchAcceleratorTools.Application](../CleanArchAcceleratorTools.Application)
- [CleanArchAcceleratorTools.Infrastructure](../CleanArchAcceleratorTools.Infrastructure)
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

### Repositories methods through repository dependency injection
- **Dynamic Selects** – Project typed objects based on property names.
- **Dynamic Filtering** – Filter queries with a fluent builder using clauses and operators.
- **Dynamic Sort** – Sort queries with multiple fields and directions.
- **Pagination Utilities** – Helpers for `IQueryable` including filtering and ordering.
- **Repositories Methods** Use all repositories methods without any extra code.
- **Extensible & Lightweight** – Modular, discoverable, and easy to integrate.

### Mediator pattern for domain events and notifications.
- **Domain Events** – Define and dispatch events for domain changes.
- **Domain Notifications** – Publish notifications for domain events.

### Exceptions
- **DomainException** – Custom exceptions for domain errors.

### QuickSearch
- Mark properties for quick search functionality. It searches all the entities properties marked with `[QuickSearch]` recursively, restricted to one-to-one relationships.

### Models
- **DynamicFilter** – Class for building dynamic filters for queries.
- **DynamicSort** – Class for building dynamic sort orders for queries.
- **Entity** base Class for domain entities with common properties.
- **PaginationResult** – Class for paginated query results.
- **ParallelParams** – Support for parallel query execution.
- **QueryFilter** – Class for encapsulating query filter parameters used in pagination.

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

- [FluentValidation](https://fluentvalidation.net/) 11.x (see version in the `.csproj`)

---

### 🚀 Installation
``` bash
dotnet add package CleanArchAcceleratorTools.Domain --version 1.0.0
```

## ⚡ Quickstart

1. If you have separated Infrastructure and Application projects add this module reference on it. Add allInOne if using a single project.
2. Model your Entities inheriting from `Entity`.
3. Add validations with FluentValidation (use `.resx` files for messages to standardize).
4. Create your services inheriting from `IGenericDomainService<TEntity>` and `GenericDomainService<TEntity>`.
5. Use all available repository methods without extra code.
6. Create your custom methods if needed.
7. Enjoy all features provided!

---

## 🛠️ Usage Examples

### Creating a Domain Entity
```csharp
public class Course : Entity
{
    public long? InstructorId { get; set; }
    public string? Title { get; set; }
    public Instructor? Instructor { get; set; }
    public ICollection<Module>? Modules { get; set; }

    public Course()
    {
    
    }
}
```

### Creating a Domain Service
```csharp
public interface ICourseDomainService : IGenericDomainService<Course> { }

public class CourseDomainService : GenericDomainService<Course>, ICourseDomainService
{
    private readonly ICourseRepository _courseRepository;

    public CourseDomainService(
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notifications,
        ICourseRepository courseRepository,
        IValidator<Course> courseValidator,
        IApplicationLogger applicationLogger) : base(mediator, notifications, courseRepository, courseValidator, applicationLogger)
    {
        _courseRepository = courseRepository;
    }
}

...

// Usage
var allCourses = await _courseDomainService.GetAllAsync();
```

---

## 📚 Example Project

See the - [CleanArchAcceleratorTools.Examples](../../examples/CleanArchAcceleratorTools.Examples) project for a complete working example, including setup and advanced scenarios. It implements a fully working API with all other modules implemented.

---

## 🧠 Tips

- Prefer `DynamicSelect` to reduce materialization and network payloads.
- Centralize selection fields in a constant for reuse as in [CourseSelects](<../../examples/CleanArchAcceleratorTools.Examples/3 - Domain/Aggregates/Courses/Selects/CourseSelects.cs>).

---

## 🤝 Contributing

Contributions are welcome! Open issues or submit PRs for features, bug fixes, or documentation improvements.
Contributing guidelines are under construction.