using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;

public class ModuleViewModel : EntityViewModel
{
    #region Simple Properties

    public long? CourseId { get; set; }
    public string? Name { get; set; }

    #endregion

    #region Navigation Properties

    #region Collections

    public ICollection<LessonViewModel>? Lessons { get; set; }

    #endregion

    #endregion
}
