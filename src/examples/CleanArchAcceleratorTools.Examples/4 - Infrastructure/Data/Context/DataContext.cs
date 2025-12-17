using CleanArchAcceleratorTools.Examples.Configs;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings;
using CleanArchAcceleratorTools.Infrastructure.Mapping;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;

public class DataContextFactory : IDbContextFactory<DataContext>
{
    private readonly IConfiguration? _configuration;

    public DataContextFactory(IConfiguration? configuration = null)
    {
        _configuration = configuration;
    }

    public virtual DataContext CreateDbContext()
    {
        if (_configuration is null)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
             .UseInMemoryDatabase(Guid.NewGuid().ToString())
             .Options;

            var context = new DataContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        var appConfig = _configuration.GetSection("ApiConfiguration");
        var config = appConfig.Get<ApiSettings>();

        var optionsBuilder = new DbContextOptionsBuilder();
        var connectionString = config?.ConnectionConfiguration.ConnectionStrings.DefaultConnection;
        var timeout = config?.ConnectionConfiguration.MaxTimeoutInSeconds;

        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseSqlServer(connectionString, options =>
        {
            options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(Convert.ToDouble(timeout)), errorNumbersToAdd: null);
            options.CommandTimeout(timeout);
        });

        return new DataContext(optionsBuilder.Options);
    }
}

public class DbContextRegistratorServiceAdapter : IDbContextRegistratorService<DbContext>
{
    private readonly IDbContextRegistratorService<DataContext> _innerRegistrator;

    public DbContextRegistratorServiceAdapter(IDbContextRegistratorService<DataContext> innerRegistrator)
    {
        _innerRegistrator = innerRegistrator;
    }

    public DbContext Create(string key)
    {
        return _innerRegistrator.Create(key);
    }

    public DbContext CreateOrReuse(string key)
    {
        return _innerRegistrator.CreateOrReuse(key);
    }

    public bool Register(string key, DbContext context)
    {
        var innerContext = context as DataContext ?? throw new InvalidOperationException("Invalid DbContext type for registration.");
        return _innerRegistrator.Register(key, innerContext);
    }
}

public class DbContextFactoryAdapter : IDbContextFactory<DbContext>
{
    private readonly IDbContextFactory<DataContext> _innerFactory;

    public DbContextFactoryAdapter(IDbContextFactory<DataContext> innerFactory)
    {
        _innerFactory = innerFactory;
    }

    public DbContext CreateDbContext()
    {
        return _innerFactory.CreateDbContext();
    }
}

public class DataContext : DbContext
{
    #region DbSets

    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Lesson> Lessons { get; set; }
    public virtual DbSet<Module> Modules { get; set; }
    public virtual DbSet<Instructor> Instructors { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<City> Cyties { get; set; }
    public virtual DbSet<State> States { get; set; }
    public virtual DbSet<Country> Countries { get; set; }

    #endregion

    public DataContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.RegisterModelsMapping(MappingsHolder.GetMappings());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Used only for ef-migration.bat
        //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CleanArchAcceleratorToolsDb;Trusted_Connection=True;MultipleActiveResultSets=true", options =>
        //{
        //    var timeout = 60;
        //    options.EnableRetryOnFailure();
        //    options.CommandTimeout(timeout);
        //});

        base.OnConfiguring(optionsBuilder);
    }
}
