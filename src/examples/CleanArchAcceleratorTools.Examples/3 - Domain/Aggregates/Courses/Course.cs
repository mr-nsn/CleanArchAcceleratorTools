using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicFilters;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;

public class Course : Entity
{
    #region Simple Properties

    public long? InstructorId { get; set; }

    [QuickSearch]
    public string? Title { get; set; }

    #endregion

    #region Navigation Properties

    [QuickSearch]
    public Instructor? Instructor { get; set; }

    #region Collections

    public ICollection<Module>? Modules { get; set; }

    #endregion

    #endregion

    #region Constructors

    public Course()
    {
    
    }

    #endregion
}
