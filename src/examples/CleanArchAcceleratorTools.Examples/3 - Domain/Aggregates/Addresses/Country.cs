using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

public class Country : Entity
{
    #region Simple Properties

    public string? Code { get; set; }
    public string? Name { get; set; }

    #endregion

    #region Constructors

    public Country()
    {

    }

    #endregion
}
