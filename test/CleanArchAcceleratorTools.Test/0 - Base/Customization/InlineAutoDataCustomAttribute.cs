using AutoFixture.Xunit2;

namespace CleanArchAcceleratorTools.Test.Customization;

public class InlineAutoDataCustomAttribute : InlineAutoDataAttribute
{
    public InlineAutoDataCustomAttribute(params object[] objects) : base(new AutoDataCustomAttribute(), objects)
    {
        
    }
}
