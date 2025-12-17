# CleanArchAcceleratorTools.AllInOne

This module is part of `CleanArchAcceleratorTools`, a toolkit to accelerate building applications with Clean Architecture. This encapsulates all other modules in one package for easier integration in projects that do not require separation of concerns.

Related modules:
- [CleanArchAcceleratorTools.Controller](../CleanArchAcceleratorTools.Controller)
- [CleanArchAcceleratorTools.Application](../CleanArchAcceleratorTools.Application)
- [CleanArchAcceleratorTools.Domain](../CleanArchAcceleratorTools.Domain)
- [CleanArchAcceleratorTools.Infrastructure](../CleanArchAcceleratorTools.Infrastructure)

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
dotnet add package CleanArchAcceleratorTools.AllInOne --version 1.0.0
```

## ⚡ Quickstart

1. Install the package
2. Use each module features as needed in your project (See each module documentation).

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