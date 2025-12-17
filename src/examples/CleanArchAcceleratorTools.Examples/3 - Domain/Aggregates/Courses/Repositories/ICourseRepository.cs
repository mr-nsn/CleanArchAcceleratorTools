using CleanArchAcceleratorTools.Domain.Interfaces.Repository;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<ICollection<Course>> GetAllParallelAsync();
    Task AddInstructorsProcedureAsync(ICollection<Instructor> instructors);
}
