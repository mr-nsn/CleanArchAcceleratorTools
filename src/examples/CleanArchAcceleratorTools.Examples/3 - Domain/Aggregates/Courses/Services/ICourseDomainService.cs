using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Domain.Services;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Services;

public interface ICourseDomainService : IGenericDomainService<Course>
{
    Task<ICollection<Course>> GetAllParallelAsync();
    Task AddInstructorsProcedureAsync(ICollection<Instructor> instructors);
}
