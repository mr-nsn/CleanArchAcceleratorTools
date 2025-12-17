# CleanArchAcceleratorTools.Examples — Quick Start

Run the ready-to-use ASP.NET Core Web API example powered by EF Core. Targets .NET 9.0.

## Prerequisites
- .NET SDK 9.0 (install from https://dotnet.microsoft.com/download)
- Optional for SQL Server mode: a SQL Server instance

## Run with Visual Studio 2022
1. Open the solution in Visual Studio 2022.
2. In Solution Explorer, right-click `CleanArchAcceleratorTools.Examples` → __Set as Startup Project__.
3. Press __Start Debugging__ (F5) or __Start Without Debugging__ (Ctrl+F5).
4. When the app starts, navigate to the Swagger UI at `/swagger` (check the console output for the exact URL, e.g., https://localhost:xxxxx/swagger).

## Run with .NET CLI
From the repository root:

```bash
dotnet restore src/examples/CleanArchAcceleratorTools.Examples/CleanArchAcceleratorTools.Examples.csproj
dotnet build src/examples/CleanArchAcceleratorTools.Examples/CleanArchAcceleratorTools.Examples.csproj
dotnet run --project src/examples/CleanArchAcceleratorTools.Examples/CleanArchAcceleratorTools.Examples.csproj -f net9.0
```
Check the console for the listening URLs, then open `/swagger`.

## Database options
This example includes both EF Core InMemory and SQL Server packages. Typical usage:
- InMemory (no setup): Keep/enable the `UseInMemoryDatabase(...)` registration in `Program.cs`.
- SQL Server (optional):
  1. Add a connection string to `appsettings.json`:
    ```
    {
      "ApiConfiguration": {
        "ConnectionConfiguration": {
          "MaxTimeoutInSeconds": 60,
          "ConnectionStrings": {
            "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchAcceleratorToolsDb;Trusted_Connection=True;MultipleActiveResultSets=true"
          }
        }
      }
    }
    ```
  2. Exclude existing migrations from the project if exists (delete the `Migrations` folder under `src/examples/CleanArchAcceleratorTools.Examples/Migrations`).
  3. Apply migrations:
    ```
    // In root folder
    ./ef-migration.bat InitialCreation
    ```

## Troubleshooting
- SDK not found: Ensure .NET 9.0 SDK is installed (`dotnet --info`).
- Swagger not loading: Confirm the app URL/port from the console output and that `/swagger` is enabled.
- SQL connection issues: Verify the connection string and `TrustServerCertificate=True` for local dev if needed.

## Learn more
- [CleanArchAcceleratorTools.Controller](../../core/CleanArchAcceleratorTools.Controller)
- [CleanArchAcceleratorTools.Application](../../core/CleanArchAcceleratorTools.Application)
- [CleanArchAcceleratorTools.Domain](../../core/CleanArchAcceleratorTools.Domain)
- [CleanArchAcceleratorTools.Infrastructure](../../core/CleanArchAcceleratorTools.Infrastructure)
- [CleanArchAcceleratorTools.AllInOne](../../core/CleanArchAcceleratorTools.AllInOne)