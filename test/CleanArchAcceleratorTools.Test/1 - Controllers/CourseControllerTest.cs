using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Customization;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Controller.Models;

namespace CleanArchAcceleratorTools.Test.Controller;

[Collection(nameof(CourseCollection))]
public class CourseControllerTest
{
    private readonly CourseFixture _courseFixture;

    public CourseControllerTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "CourseController - AddAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddAsync_ShouldReturnOk(CourseViewModel course)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.AddAndCommitAsync(course))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ControllerImpl.AddAsync(course);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returned = Assert.IsType<ResponseResult>(okResult.Value);
        Assert.NotNull(returned.Data);
        Assert.Equal(course.Id, returned.ToTyped<CourseViewModel>().Data?.Id);
    }

    [Theory(DisplayName = "CourseController - AddRangeAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddRangeAsync_ShouldReturnOk(ICollection<CourseViewModel> courses)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.AddRangeAndCommitAsync(courses))
            .ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.ControllerImpl.AddRangeAsync(courses);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returned = Assert.IsType<ResponseResult>(okResult.Value);
        var data = Assert.IsAssignableFrom<ICollection<CourseViewModel>>(returned.ToTyped<ICollection<CourseViewModel>>().Data);
        Assert.Equal(courses.Count, data.Count);
    }

    [Theory(DisplayName = "CourseController - DynamicSelectAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task DynamicSelectAsync_ShouldReturnOk(ICollection<CourseViewModel> courses)
    {
        // Arrange
        var dynamicSelect = _courseFixture.GenerateValidDynamicSelectViewModel();

        _courseFixture.ApplicationServiceMock
            .Setup(x => x.DynamicSelectAsync(dynamicSelect.DynamicFilter, dynamicSelect.DynamicSort, dynamicSelect.Fields))
            .ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.ControllerImpl.DynamicSelectAsync(dynamicSelect);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Theory(DisplayName = "CourseController - GetAllAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetAllAsync_ShouldReturnOk(ICollection<CourseViewModel> courses)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.ControllerImpl.GetAllAsync();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Theory(DisplayName = "CourseController - GetByIdAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetByIdAsync_ShouldReturnOk(long? id, CourseViewModel course)
    {
        // Arrange
        course.Id = id;

        _courseFixture.ApplicationServiceMock
            .Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ControllerImpl.GetByIdAsync(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returned = Assert.IsType<ResponseResult>(okResult.Value);
        Assert.NotNull(returned.Data);
        Assert.Equal(id, returned.ToTyped<CourseViewModel>().Data?.Id);
    }

    [Theory(DisplayName = "CourseController - RemoveAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveAsync_ShouldReturnOk(long? id)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.RemoveAndCommitAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _courseFixture.ControllerImpl.RemoveAsync(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Theory(DisplayName = "CourseController - RemoveRangeAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveRangeAsync_ShouldReturnOk(long?[] ids)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.RemoveRangeAndCommitAsync(ids))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _courseFixture.ControllerImpl.RemoveRangeAsync(ids);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Theory(DisplayName = "CourseController - SearchWithPaginationAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task SearchWithPaginationAsync_ShouldReturnOk(PaginationResultViewModel<CourseViewModel> courses)
    {
        // Arrange
        var query = _courseFixture.GenerateValidQueryFilterViewModel();

        _courseFixture.ApplicationServiceMock
            .Setup(x => x.SearchWithPaginationAsync(query))
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.ControllerImpl.SearchWithPaginationAsync(query);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Theory(DisplayName = "CourseController - UpdateAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateAsync_ShouldReturnOk(CourseViewModel course)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.UpdateAndCommitAsync(course))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ControllerImpl.UpdateAsync(course);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returned = Assert.IsType<ResponseResult>(okResult.Value);
        Assert.NotNull(returned.Data);
        Assert.Equal(course.Id, returned.ToTyped<CourseViewModel>().Data?.Id);
    }

    [Theory(DisplayName = "CourseController - UpdateRangeAsync - Should return ok")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateRangeAsync_ShouldReturnOk(ICollection<CourseViewModel> courses)
    {
        // Arrange
        _courseFixture.ApplicationServiceMock
            .Setup(x => x.UpdateRangeAndCommitAsync(courses))
            .ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.ControllerImpl.UpdateRangeAsync(courses);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        var returned = Assert.IsType<ResponseResult>(okResult.Value);
        var data = Assert.IsAssignableFrom<ICollection<CourseViewModel>>(returned.ToTyped<ICollection<CourseViewModel>>().Data);
        Assert.Equal(courses.Count, data.Count);
    }
}
