using CleanArchAcceleratorTools.Application.Services;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Infrastructure.UOW;
using Mapster;

namespace CleanArchAcceleratorTools.Examples.Application.Aggregates.Courses.Services;

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

    public async Task<ICollection<CourseViewModel>> GetAllParallelAsync()
    {
        var courses = await _courseDomainService.GetAllParallelAsync();
        return courses.Adapt<ICollection<CourseViewModel>>();
    }

    public async Task AddInstructorsProcedureAsync(ICollection<InstructorViewModel> instructors)
    {
        var instructorEntities = instructors.Adapt<ICollection<Instructor>>();
        await _courseDomainService.AddInstructorsProcedureAsync(instructorEntities);
    }
}
