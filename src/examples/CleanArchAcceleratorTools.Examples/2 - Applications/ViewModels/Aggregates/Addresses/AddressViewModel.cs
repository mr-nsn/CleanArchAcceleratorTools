using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Addresses;

public class AddressViewModel : EntityViewModel
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

    public CityViewModel? City { get; set; }

    #endregion
}