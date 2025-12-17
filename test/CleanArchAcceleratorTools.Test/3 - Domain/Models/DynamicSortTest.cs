using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class DynamicSortTest
{
    private readonly CourseFixture _courseFixture;

    public DynamicSortTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "DynamicSort - SetFieldsOrder - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetFieldsOrder_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var dynamicSort = new DynamicSort<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dynamicSort.SetFieldsSort(null));
    }

    [Theory(DisplayName = "DynamicSort - AddFieldOrder - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddFieldOrder_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var dynamicSort = new DynamicSort<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dynamicSort.AddFieldSort(null, null));
    }    
}
