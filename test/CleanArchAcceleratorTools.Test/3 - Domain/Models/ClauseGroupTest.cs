using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;

namespace CleanArchAcceleratorTools.Test.Domain.Models;

[Collection(nameof(CourseCollection))]
public class ClauseGroupTest
{
    private readonly CourseFixture _courseFixture;

    public ClauseGroupTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "ClauseGroup - AddClause - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddClause_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clauseGroup = new ClauseGroup<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clauseGroup.AddClause(null));
    }

    [Theory(DisplayName = "ClauseGroup - SetClauses - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetClauses_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clause = new ClauseGroup<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clause.SetClauses(null));
    }

    [Theory(DisplayName = "ClauseGroup - SetLogicOperator - Should throw exception when argument is null")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SetLogicOperator_ShouldThrowExceptionWhenArgumentIsNull(string _)
    {
        // Arrange
        var clause = new ClauseGroup<Course>();

        // Act and Assert
        Assert.Throws<ArgumentNullException>(() => clause.SetLogicOperator(null));
    }    
}
