using CleanArchAcceleratorTools.Domain.Configurations;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Models.Validators;
using CleanArchAcceleratorTools.Examples.Application.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Applications.Mappers;
using CleanArchAcceleratorTools.Examples.Configs;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Repositories.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Logging;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.IoC;

public static class BootStrapper
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Configuration

        var appConfig = configuration.GetSection("ApiConfiguration");
        var config = appConfig.Get<ApiSettings>();        

        services.Configure<ApiSettings>(appConfig);        

        #endregion

        #region Context

        services.AddDbContext<DataContext>(options => {
            options.EnableSensitiveDataLogging();
            options.UseSqlServer(config?.ConnectionConfiguration.ConnectionStrings.DefaultConnection, options =>
            {
                var timeout = config?.ConnectionConfiguration.MaxTimeoutInSeconds;
                options.EnableRetryOnFailure();
                options.CommandTimeout(timeout);
            });
        }, contextLifetime: ServiceLifetime.Scoped, optionsLifetime: ServiceLifetime.Scoped);

        services.AddDbContextFactory<DataContext, DataContextFactory>(lifetime: ServiceLifetime.Scoped);

        #endregion

        #region CleaArch

        // Configuring DbContext and DbContextFactory
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<DataContext>());
        services.AddScoped<IDbContextFactory<DbContext>>(sp => new DbContextFactoryAdapter(sp.GetRequiredService<IDbContextFactory<DataContext>>()));

        services.AddCleanArchConfiguration<DataContext>();

        #endregion

        #region Domain

        services.AddScoped<ICourseDomainService, CourseDomainService>();

        #region Validations

        services.AddScoped<IValidator<Course>, CourseValidator>();

        #endregion

        #endregion

        #region Application

        services.AddScoped<ICourseApplicationService, CourseApplicationService>();

        #endregion

        #region Repositories

        services.AddScoped<ICourseRepository, CourseRepository>();

        #endregion

        #region Logging

        services.AddScoped<IApplicationLogger, ApplicationLogger>();

        #endregion
    }

    public static void RegisterMappers(this IServiceCollection services)
    {
        // Mapster configuration
        ViewModelToDomainMapper.AddMapping();
        DomainToViewModelMapper.AddMapping();
        //NullableMapper.AddMapping();
    }
}
