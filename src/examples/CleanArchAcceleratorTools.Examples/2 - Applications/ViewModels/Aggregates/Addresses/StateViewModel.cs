using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Addresses;

public class StateViewModel : EntityViewModel
{
    #region Simple Properties

    public long? CountryId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Abbreviation { get; set; }

    #endregion

    #region Navigation Properties

    public CountryViewModel? Country { get; set; }

    #endregion
}