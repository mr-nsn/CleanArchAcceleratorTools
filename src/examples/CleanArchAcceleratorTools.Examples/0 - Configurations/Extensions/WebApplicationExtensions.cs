using CleanArchAcceleratorTools.Examples.Infrastructure.Data;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;

namespace CleanArchAcceleratorTools.Examples.Configurations.Extensions;

public static class WebApplicationExtensions
{
    public static void SeedContext(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            var context = services.GetRequiredService<DataContext>();
            DbInitializer.SeedAsync(context).GetAwaiter().GetResult();
        }
    }
}
