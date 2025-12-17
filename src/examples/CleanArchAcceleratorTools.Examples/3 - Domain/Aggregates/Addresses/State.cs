using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

public class State : Entity
{
    #region Simple Properties

    public long? CountryId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? Abbreviation { get; set; } = string.Empty;

    #endregion

    #region Navigation Properties

    public Country? Country { get; set; }

    #endregion

    #region Constructors

    public State()
    {
    }

    #endregion
}