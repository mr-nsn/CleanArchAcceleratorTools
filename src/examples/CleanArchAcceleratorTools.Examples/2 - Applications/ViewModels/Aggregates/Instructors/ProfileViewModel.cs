using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Addresses;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;

public class ProfileViewModel : EntityViewModel
{
    #region Simple Properties

    public long? InstructorId { get; set; }
    public long? AddressId { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }

    #endregion

    #region Navigation Properties

    public AddressViewModel? Address { get; set; }

    #endregion

    #region Constructors

    public ProfileViewModel()
    {

    }

    #endregion
}
