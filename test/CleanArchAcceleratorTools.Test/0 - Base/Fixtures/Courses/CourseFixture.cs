using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Models.Builders;
using CleanArchAcceleratorTools.Domain.Models.Selects.Defaults;
using CleanArchAcceleratorTools.Domain.Models.Validators;
using CleanArchAcceleratorTools.Examples.Application.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Applications.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Controllers;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Repositories.Aggregates.Courses;
using FluentValidation;
using Mapster;
using Moq;

namespace CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;

public class CourseFixture : FixtureBase
{
    public Mock<ICourseApplicationService> ApplicationServiceMock { get; private set; }
    public Mock<ICourseDomainService> DomainServiceMock { get; private set; }
    public Mock<IValidator<Course>> ValidatorMock { get; private set; }
    public Mock<ICourseRepository> RepositoryMock { get; private set; }

    public CoursesController ControllerImpl { get; private set; }
    public CourseApplicationService ApplicationServiceImpl { get; private set; }
    public CourseDomainService DomainServiceImpl { get; private set; }
    public IValidator<Course> ValidatorImpl { get; private set; }
    public CourseRepository RepositoryImpl { get; private set; }

    public CourseFixture()
    {
        ApplicationServiceMock = new Mock<ICourseApplicationService>();
        DomainServiceMock = new Mock<ICourseDomainService>();
        ValidatorMock = new Mock<IValidator<Course>>();
        RepositoryMock = new Mock<ICourseRepository>();

        ControllerImpl = new CoursesController(
            MediatorMock.Object,
            NotificationsMock.Object,
            ApplicationLoggerMock.Object,
            ApplicationServiceMock.Object
        );

        ApplicationServiceImpl = new CourseApplicationService(
            RepositoryMock.Object,
            DomainServiceMock.Object,
            UnitOfWorkMock.Object,
            MediatorMock.Object,
            NotificationsMock.Object,
            ApplicationLoggerMock.Object
        );

        ValidatorImpl = new CourseValidator();

        DomainServiceImpl = new CourseDomainService(
            MediatorMock.Object,
            NotificationsMock.Object,
            RepositoryMock.Object,
            ValidatorImpl,
            ApplicationLoggerMock.Object
        );

        RepositoryImpl = new CourseRepository(
            Context,
            DbContextRegistratorServiceMock.Object,
            ApplicationLoggerMock.Object
        );
    }

    public override void Reset()
    {
        base.Reset();

        ApplicationServiceMock = new Mock<ICourseApplicationService>();
        DomainServiceMock = new Mock<ICourseDomainService>();
        RepositoryMock = new Mock<ICourseRepository>();

        ControllerImpl = new CoursesController(
            MediatorMock.Object,
            NotificationsMock.Object,
            ApplicationLoggerMock.Object,
            ApplicationServiceMock.Object
        );

        ApplicationServiceImpl = new CourseApplicationService(
            RepositoryMock.Object,
            DomainServiceMock.Object,
            UnitOfWorkMock.Object,
            MediatorMock.Object,
            NotificationsMock.Object,
            ApplicationLoggerMock.Object
        );

        ValidatorImpl = new CourseValidator();

        DomainServiceImpl = new CourseDomainService(
            MediatorMock.Object,
            NotificationsMock.Object,
            RepositoryMock.Object,
            ValidatorImpl,
            ApplicationLoggerMock.Object
        );

        RepositoryImpl = new CourseRepository(
            Context,
            DbContextRegistratorServiceMock.Object,
            ApplicationLoggerMock.Object
        );
    }    

    public QueryFilterViewModel<CourseViewModel> GenerateValidQueryFilterViewModel()
    {
        var queryFilter = GenerateValidQueryFilter();
        return queryFilter.Adapt<QueryFilterViewModel<CourseViewModel>>();
    }

    public QueryFilter<Course> GenerateValidQueryFilter()
    {
        var queryFilter = new QueryFilterBuilder<Course>()
            .WithPage(1)
            .WithPageSize(10)
            .WithFields(SelectsDefaults<Course>.BasicFields)
            .Build();

        return queryFilter;
    }

    public CourseDinamicSelectViewModel GenerateValidDynamicSelectViewModel()
    {
        var dynamicSelectViewModel = new CourseDinamicSelectViewModel()
        {
            DynamicFilter = default,
            DynamicSort = default,
            Fields = SelectsDefaults<Course>.BasicFields
        };

        return dynamicSelectViewModel;
    }
}
