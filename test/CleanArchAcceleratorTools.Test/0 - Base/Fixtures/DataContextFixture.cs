using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Test.Base.Fixtures;

public class DataContextFixture
{
    public DataContext GetDataContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new DataContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }

    public DataContextFactory GetDataContextFactory()
    {
        return new DataContextFactory();
    }
}