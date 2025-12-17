using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class DynamicFilterTest
{
    private readonly CourseFixture _courseFixture;

    public DynamicFilterTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "DynamicFilter - AddClause - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddClause_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var dynamicFilter = new DynamicFilter<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dynamicFilter.AddClauseGroup(null));
    }

    [Theory(DisplayName = "DynamicFilter - SetClauseGroups - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetClauseGroups_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var dynamicFilter = new DynamicFilter<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => dynamicFilter.SetClauseGroups(null));
    }    
}
