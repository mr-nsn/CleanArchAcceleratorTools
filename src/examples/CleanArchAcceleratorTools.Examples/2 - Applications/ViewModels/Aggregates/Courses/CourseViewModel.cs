using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;

public class CourseViewModel : EntityViewModel
{
    #region Simple Properties

    public long? InstructorId { get; set; }

    public string? Title { get; set; }

    #endregion

    #region Navigation Properties

    public InstructorViewModel? Instructor { get; set; }

    #region Collections

    public ICollection<ModuleViewModel>? Modules { get; set; }

    #endregion

    #endregion

}