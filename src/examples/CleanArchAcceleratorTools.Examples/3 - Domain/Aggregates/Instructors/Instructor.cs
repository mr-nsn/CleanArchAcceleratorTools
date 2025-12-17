using CleanArchAcceleratorTools.Domain.Constants;
using CleanArchAcceleratorTools.Domain.DynamicFilters;
using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;

public class Instructor : Entity
{
    #region Simple Properties

    [QuickSearch]
    public string? FullName { get; set; }

    #endregion

    #region Navigation Properties

    [QuickSearch]
    public Profile? Profile { get; set; }

    #endregion

    #region Constructors

    public Instructor()
    {

    }

    #endregion
}
