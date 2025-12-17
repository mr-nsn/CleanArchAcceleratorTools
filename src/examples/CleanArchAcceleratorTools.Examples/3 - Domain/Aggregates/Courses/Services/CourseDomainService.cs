using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Domain.Services;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using FluentValidation;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Services;

public class CourseDomainService : GenericDomainService<Course>, ICourseDomainService
{
    private readonly ICourseRepository _courseRepository;

    public CourseDomainService(
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notifications,
        ICourseRepository courseRepository,
        IValidator<Course> courseValidator,
        IApplicationLogger applicationLogger) : base(mediator, notifications, courseRepository, courseValidator, applicationLogger)
    {
        _courseRepository = courseRepository;
    }

    public async Task<ICollection<Course>> GetAllParallelAsync()
    {
        return await _courseRepository.GetAllParallelAsync();
    }

    public async Task AddInstructorsProcedureAsync(ICollection<Instructor> instructors)
    {
        await _courseRepository.AddInstructorsProcedureAsync(instructors);
    }
}
