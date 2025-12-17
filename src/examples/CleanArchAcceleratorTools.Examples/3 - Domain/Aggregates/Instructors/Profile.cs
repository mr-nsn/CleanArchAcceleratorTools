using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicFilters;
using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;

public class Profile : Entity
{
    #region Simple Properties

    public long? InstructorId { get; set; }
    public long? AddressId { get; set; }
    public string? Bio { get; set; }

    [QuickSearch]
    public string? LinkedInUrl { get; set; }

    #endregion

    #region Navigation Properties

    public Address? Address { get; set; }

    #endregion

    #region Constructors

    public Profile()
    {

    }

    #endregion
}