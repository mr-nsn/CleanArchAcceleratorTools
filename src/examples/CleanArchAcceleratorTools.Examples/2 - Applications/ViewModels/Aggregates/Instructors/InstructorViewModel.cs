using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;

public class InstructorViewModel : EntityViewModel
{
    #region Simple Properties

    public string? FullName { get; set; }

    #endregion

    #region Navigation Properties

    public ProfileViewModel? Profile { get; set; }

    #endregion
}
