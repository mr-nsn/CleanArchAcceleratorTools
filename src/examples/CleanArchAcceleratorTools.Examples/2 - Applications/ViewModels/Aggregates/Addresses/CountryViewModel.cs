using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Addresses;

public class CountryViewModel : EntityViewModel
{
    #region Simple Properties

    public string? Code { get; set; }
    public string? Name { get; set; }

    #endregion
}