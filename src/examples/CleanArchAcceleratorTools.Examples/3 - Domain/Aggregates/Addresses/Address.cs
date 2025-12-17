using CleanArchAcceleratorTools.Domain.Models;

namespace CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;

public class Address : Entity
{
    #region Simple Properties

    public long? CityId { get; set; }
    public string? StreetAvenue { get; set; }
    public string? Number { get; set; }
    public string? Complement { get; set; }
    public string? Neighborhood { get; set; }
    public string? PostalCode { get; set; }

    #endregion

    #region Navigation Properties

    public City? City { get; set; }

    #endregion

    #region Constructors

    public Address()
    {

    }

    #endregion
}