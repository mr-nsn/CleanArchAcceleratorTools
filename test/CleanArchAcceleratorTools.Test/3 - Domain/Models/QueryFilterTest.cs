using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class QueryFilterTest
{
    private readonly CourseFixture _courseFixture;

    public QueryFilterTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "QueryFilter - SetFields - Should throw exception when argument empty")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetFields_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var queryFilter = new QueryFilter<Course>();

        // Act and Assert
        Assert.Throws<ArgumentException>(() => queryFilter.SetFields());
    }

    [Theory(DisplayName = "QueryFilter - SetPage - Should throw exception when argument is less than 1")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetPage_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var queryFilter = new QueryFilter<Course>();

        // Act and Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => queryFilter.SetPage(0));
    }

    [Theory(DisplayName = "QueryFilter - SetPageSize - Should throw exception when argument is less than 1")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetPageSize_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var queryFilter = new QueryFilter<Course>();

        // Act and Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => queryFilter.SetPageSize(0));
    }
}
