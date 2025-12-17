using CleanArchAcceleratorTools.Application.ViewModels;

namespace CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Addresses;

public class CityViewModel : EntityViewModel
{
    #region Simple Properties

    public long? StateId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }

    #endregion

    #region Navigation Properties

    public StateViewModel? State { get; set; }

    #endregion
}