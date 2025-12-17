using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class ClauseTest
{
    private readonly CourseFixture _courseFixture;

    public ClauseTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "Clause - SetComparisonOperator - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetComparisonOperator_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clause = new Clause<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clause.SetComparisonOperator(null));
    }

    [Theory(DisplayName = "Clause - SetField - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetField_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clause = new Clause<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clause.SetField(null));
    }

    [Theory(DisplayName = "Clause - SetLogicOperator - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetLogicOperator_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clause = new Clause<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clause.SetLogicOperator(null));
    }    
}
