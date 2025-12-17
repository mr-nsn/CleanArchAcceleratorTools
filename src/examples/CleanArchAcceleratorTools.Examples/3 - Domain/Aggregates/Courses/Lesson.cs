using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;

public class Lesson : Entity
{
    #region Simple Properties

    public long? ModuleId { get; set; }
    public string? Title { get; set; }
    public TimeSpan? Duration { get; set; }

    #endregion

    #region Constructors

    public Lesson()
    {

    }

    #endregion
}
