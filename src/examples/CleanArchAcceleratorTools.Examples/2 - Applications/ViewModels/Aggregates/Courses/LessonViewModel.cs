using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;

public class LessonViewModel : EntityViewModel
{
    #region Simple Properties

    public long? ModuleId { get; set; }
    public string? Title { get; set; }
    public TimeSpan? Duration { get; set; }

    #endregion
}
