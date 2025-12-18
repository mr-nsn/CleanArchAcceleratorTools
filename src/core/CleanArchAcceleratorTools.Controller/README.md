# CleanArchAcceleratorTools.Controller

This module is part of `CleanArchAcceleratorTools`, a toolkit to accelerate building applications with Clean Architecture. The Controller module contains controllers to expose endpoints, handling HTTP requests and responses. It references the Application layer to delegate business logic and data operations.

Related modules:
- [CleanArchAcceleratorTools.Application](../CleanArchAcceleratorTools.Application)
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

### Repositories methods through application dependency injection
- **Dynamic Selects** – Project typed objects based on property names.
- **Dynamic Filtering** – Filter queries with a fluent builder using clauses and operators.
- **Dynamic Sort** – Sort queries with multiple fields and directions.
- **Pagination Utilities** – Helpers for `IQueryable` including filtering and ordering.
- **Repositories Methods** - Use all repositories methods only calling the application layer in your controller.
- **Extensible & Lightweight** – Modular, discoverable, and easy to integrate.

### Global Exception filtering
- **Exception Handling Middleware** – Centralized exception handling for consistent error responses.

### Access to Application ViewModels
- **DynamicFilterViewModel** – Class for building dynamic filters for queries.
- **DynamicSortViewModel** – Class for building dynamic sort orders for queries.
- **EntityViewModel** base Class for domain entities with common properties.
- **PaginationResultViewModel** – Class for paginated query results.
- **QueryFilterViewModel** – Class for encapsulating query filter parameters used in pagination.

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
dotnet add package CleanArchAcceleratorTools.Controller --version x.x.x
```

## ⚡ Quickstart

1. Create your controllers inheriting from `GenericController<TEntity, TEntityViewModel>`.
2. Create the enpoints.
3. Use the application service methods in your controller actions.
4. Create your custom methods if needed.
5. Enjoy all features provided!

---

## 🛠️ Usage Examples

### Configuring global exception filter and NewtonsoftJson (optional)
```Csharp
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter)); // Register ExceptionFilter globally
}).AddNewtonsoftJson(config =>
{
    config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    config.SerializerSettings.Formatting = Formatting.Indented;
    config.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});
```

### Creating a controller
```csharp
[ApiController]
[Route("courses")]
public class CoursesController : GenericController<Course, CourseViewModel>
{
    private readonly ICourseApplicationService _courseApplicationService;

    public CoursesController(
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notificationHandler,
        IApplicationLogger applicationLogger,
        ICourseApplicationService courseApplicationService
    ) : base(courseApplicationService, mediator, notificationHandler, applicationLogger)
    {
        _courseApplicationService = courseApplicationService;
    }

    [HttpPost("search-with-pagination")]
    public async Task<IActionResult> SearchWithPaginationAsync(QueryFilterViewModel<CourseViewModel> queryFilterViewModel)
    {
        var result = await _courseApplicationService.SearchWithPaginationAsync(queryFilterViewModel);
        return ResponseResult(result);
    }
}

...

// Usage
curl -X 'POST' \
  'http://localhost:<you-port>/courses/search-with-pagination' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json-patch+json' \
  -d '{
  "page": 0,
  "pageSize": 0,
  "fields": [
    "string"
  ],
  "dynamicFilter": {
    "quickSearch": "string",
    "clauseGroups": [
      {
        "logicOperator": "string",
        "clauses": [
          {
            "logicOperator": "string",
            "field": "string",
            "comparisonOperator": "string",
            "value": "string"
          }
        ]
      }
    ]
  },
  "dynamicSort": {
    "fieldsOrder": [
      {
        "field": "string",
        "order": "string"
      }
    ]
  }
}'
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