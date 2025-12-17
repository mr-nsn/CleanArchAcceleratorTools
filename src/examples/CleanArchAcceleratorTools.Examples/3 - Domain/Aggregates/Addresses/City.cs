using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

public class City : Entity
{
    #region Simple Properties

    public long? StateId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; } = string.Empty;

    #endregion

    #region Navigation Properties

    public State? State { get; set; }

    #endregion

    #region Constructors

    public City()
    {

    }

    #endregion
}
