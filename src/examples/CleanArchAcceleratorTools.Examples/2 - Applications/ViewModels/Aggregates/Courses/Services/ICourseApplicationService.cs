using CleanArchAcceleratorTools.Application.Services;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;

namespace CleanArchAcceleratorTools.Examples.Application.Aggregates.Courses.Services;

public interface ICourseApplicationService : IGenericApplicationService<Course, CourseViewModel>
{
    Task<ICollection<CourseViewModel>> GetAllParallelAsync();
    Task AddInstructorsProcedureAsync(ICollection<InstructorViewModel> instructors);
}
