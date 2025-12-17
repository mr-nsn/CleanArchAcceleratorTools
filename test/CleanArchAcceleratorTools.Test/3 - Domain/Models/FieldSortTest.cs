using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class FieldSortTest
{
    private readonly CourseFixture _courseFixture;

    public FieldSortTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "FieldSort - SetField - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetField_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var fieldSort = new FieldSort<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => fieldSort.SetField(null));
    }

    [Theory(DisplayName = "FieldSort - SetOrder - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetOrder_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var fieldSort = new FieldSort<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => fieldSort.SetOrder(null));
    }    
}
