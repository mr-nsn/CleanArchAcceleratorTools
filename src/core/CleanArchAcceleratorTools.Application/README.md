# CleanArchAcceleratorTools.Application

This module is part of `CleanArchAcceleratorTools`, a toolkit to accelerate building applications with Clean Architecture. The Application module contains application services, DTOs/ViewModels, and orchestrates use cases. It references the Domain and repository layers to implement business logic without direct data access.

Related modules:
- [CleanArchAcceleratorTools.Controller](../CleanArchAcceleratorTools.Controller)
- [CleanArchAcceleratorTools.Domain](../CleanArchAcceleratorTools.Domain)
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

### Repositories methods through repository and domain services dependency injection
- **Dynamic Selects** – Project typed objects based on property names.
- **Dynamic Filtering** – Filter queries with a fluent builder using clauses and operators.
- **Dynamic Sort** – Sort queries with multiple fields and directions.
- **Pagination Utilities** – Helpers for `IQueryable` including filtering and ordering.
- **Repositories Methods** Use all repositories methods without any extra code.
- **Extensible & Lightweight** – Modular, discoverable, and easy to integrate.


### ViewModels
- **DynamicFilterViewModel** – Class for building dynamic filters for queries.
- **DynamicSortViewModel** – Class for building dynamic sort orders for queries.
- **EntityViewModel** base Class for domain entities with common properties.
- **PaginationResultViewModel** – Class for paginated query results.
- **QueryFilterViewModel** – Class for encapsulating query filter parameters used in pagination.

### Unit of Work
- **IUnitOfWork** – Interface for managing transactions and coordinating repository operations.
- **UnitOfWork** – Implementation of the Unit of Work pattern to ensure data integrity and consistency.
- **Manage multiple** repository operations within a single transaction.
- **Suport** to multiple DbContexts and factory pattern.

---

## 🧩 Compatibility

Multi-target:
- .NET 6
- .NET 7
- .NET 8
- .NET 9

Use an EF Core version compatible with your target framework.

---

### 🚀 Installation
``` bash
dotnet add package CleanArchAcceleratorTools.Application --version 1.0.0
```

## ⚡ Quickstart

1. If you have separated Controller/Api project add this module reference on it. Add allInOne if using a single project.
2. Model your ViewModels inheriting from `EntityViewModel`.
4. Create your services inheriting from `IGenericApplicationService<TEntity, TEntityViewModel>` and `GenericApplicationService<TEntity, TEntityViewModel>` (rememenber to add it to dependency injection).
5. Use all available repository methods without extra code.
6. Create your custom methods if needed.
7. Enjoy all features provided!

---

## 🛠️ Usage Examples

### Creating an Application ViewModel
```csharp
public class CourseViewModel : EntityViewModel
{
    public long? InstructorId { get; set; }
    public string? Title { get; set; }
    public InstructorViewModel? Instructor { get; set; }
    public ICollection<ModuleViewModel>? Modules { get; set; }
}
```

### Creating an Application Service
```csharp
public interface ICourseApplicationService : IGenericApplicationService<Course, CourseViewModel>{ }

public class CourseApplicationService : GenericApplicationService<Course, CourseViewModel>, ICourseApplicationService
{
    private readonly ICourseDomainService _courseDomainService;

    public CourseApplicationService(
        ICourseRepository courseRepository,
        ICourseDomainService domainService,
        IUnitOfWork unitOfWork,        
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notifications,
        IApplicationLogger applicationLogger) : base(courseRepository, domainService, unitOfWork, mediator, notifications, applicationLogger)
    {
        _courseDomainService = domainService;
    }
}

...

// Usage
var allCourses = await _courseApplicationService.GetAllAsync();
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