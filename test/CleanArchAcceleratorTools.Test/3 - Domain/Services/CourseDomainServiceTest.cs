using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses.Selects;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;
using Moq;

namespace CleanArchAcceleratorTools.Test.Domain.Services;

[Collection(nameof(CourseCollection))]
public class CourseDomainServiceTest
{
    private readonly CourseFixture _courseFixture;

    public CourseDomainServiceTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "CourseDomainService - Add - Should add the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Add_ShouldAddTheEntity(Course course)
    {
        // Act
        _courseFixture.DomainServiceImpl.Add(course);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.Add(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddAndCommit - Should add and commit the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddAndCommit_ShouldAddAndCommitTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddAndCommit(It.IsAny<Course>()))
            .Returns(course);

        // Act
        var result = _courseFixture.DomainServiceImpl.AddAndCommit(course);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        _courseFixture.RepositoryMock.Verify(x => x.AddAndCommit(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddAndCommitAsync - Should add and commit the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddAndCommitAsync_ShouldAddAndCommitTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddAndCommitAsync(It.IsAny<Course>()))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.DomainServiceImpl.AddAndCommitAsync(course);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        _courseFixture.RepositoryMock.Verify(x => x.AddAndCommitAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddAsync - Should add the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddAsync_ShouldAddTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.AddAsync(course);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddRange - Should add a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddRange_ShouldAddACollectionOfEntities(ICollection<Course> courses)
    {
        // Act
        _courseFixture.DomainServiceImpl.AddRange(courses);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.AddRange(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddRangeAndCommit - Should add and commit a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddRangeAndCommit_ShouldAddAndCommitACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddRangeAndCommit(It.IsAny<ICollection<Course>>()))
            .Returns(courses);

        // Act
        var result = _courseFixture.DomainServiceImpl.AddRangeAndCommit(courses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.AddRangeAndCommit(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddRangeAndCommitAsync - Should add and commit a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddRangeAndCommitAsync_ShouldAddAndCommitACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddRangeAndCommitAsync(It.IsAny<ICollection<Course>>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.DomainServiceImpl.AddRangeAndCommitAsync(courses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.AddRangeAndCommitAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - AddRangeAsync - Should add a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddRangeAsync_ShouldAddACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.AddRangeAsync(It.IsAny<ICollection<Course>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.AddRangeAsync(courses);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.AddRangeAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - DynamicSelect - Should return a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void DynamicSelect_ShouldReturnACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var fields = CourseSelects.BasicFields;

        _courseFixture.RepositoryMock
            .Setup(x => x.DynamicSelect(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()))
            .Returns(courses.ToList());

        // Act
        var result = _courseFixture.DomainServiceImpl.DynamicSelect(dynamicFilter: default, dynamicSort: default, fields);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.DynamicSelect(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - DynamicSelectAsync - Should return a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task DynamicSelectAsync_ShouldReturnACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var fields = CourseSelects.BasicFields;

        _courseFixture.RepositoryMock
            .Setup(x => x.DynamicSelectAsync(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()))
            .ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.DomainServiceImpl.DynamicSelectAsync(dynamicFilter: default, dynamicSort: default, fields);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.DynamicSelectAsync(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - GetAll - Should return a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void GetAll_ShouldReturnACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.GetAll())
            .Returns(courses);

        // Act
        var result = _courseFixture.DomainServiceImpl.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - GetAllAsync - Should return a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetAllAsync_ShouldReturnACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.DomainServiceImpl.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - GetById - Should return an entity when found")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void GetById_ShouldReturnAnEntityWhenFound(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.GetById(course.Id))
            .Returns(course);

        // Act
        var result = _courseFixture.DomainServiceImpl.GetById(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result?.Id);
        _courseFixture.RepositoryMock.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - GetByIdAsync - Should return an entity when found")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetByIdAsync_ShouldReturnAnEntityWhenFound(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.GetByIdAsync(course.Id))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.DomainServiceImpl.GetByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result?.Id);
        _courseFixture.RepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - Remove - Should remove by id")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Remove_ShouldRemoveById(long id)
    {
        // Act
        _courseFixture.DomainServiceImpl.Remove(id);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.Remove(id), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveAndCommit - Should remove by id and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveAndCommit_ShouldRemoveByIdAndCommit(long id)
    {
        // Act
        _courseFixture.DomainServiceImpl.RemoveAndCommit(id);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveAndCommit(id), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveAndCommitAsync - Should remove by id and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveAndCommitAsync_ShouldRemoveByIdAndCommit(long id)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.RemoveAndCommitAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.RemoveAndCommitAsync(id);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveAndCommitAsync(id), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveAsync - Should remove by id")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveAsync_ShouldRemoveById(long id)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.RemoveAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.RemoveAsync(id);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveAsync(id), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveRange - Should remove a range of ids")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveRange_ShouldRemoveARangeOfIds(long?[] ids)
    {
        // Act
        _courseFixture.DomainServiceImpl.RemoveRange(ids);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveRange(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveRangeAndCommit - Should remove a range of ids and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveRangeAndCommit_ShouldRemoveARangeOfIdsAndCommit(long?[] ids)
    {
        // Act
        _courseFixture.DomainServiceImpl.RemoveRangeAndCommit(ids);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveRangeAndCommit(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveRangeAndCommitAsync - Should remove a range of ids and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveRangeAndCommitAsync_ShouldRemoveARangeOfIdsAndCommit(long?[] ids)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.RemoveRangeAndCommitAsync(It.IsAny<long?[]>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.RemoveRangeAndCommitAsync(ids);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveRangeAndCommitAsync(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - RemoveRangeAsync - Should remove a range of ids")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveRangeAsync_ShouldRemoveARangeOfIds(long?[] ids)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.RemoveRangeAsync(It.IsAny<long?[]>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.RemoveRangeAsync(ids);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.RemoveRangeAsync(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - SearchWithPagination - Should return a paginated collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SearchWithPagination_ShouldReturnAPaginatedCorrespondingCollectionOfEntities(PaginationResult<Course> paginationResult)
    {
        // Arrange
        var query = _courseFixture.GenerateValidQueryFilter();

        _courseFixture.RepositoryMock
            .Setup(x => x.SearchWithPagination(It.IsAny<QueryFilter<Course>>()))
            .Returns(paginationResult);

        // Act
        var result = _courseFixture.DomainServiceImpl.SearchWithPagination(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paginationResult.Page, result.Page);
        Assert.Equal(paginationResult.PageSize, result.PageSize);
        Assert.Equal(paginationResult.TotalRecords, result.TotalRecords);
        Assert.Equal(paginationResult.Result.Count, result.Result?.Count);
        _courseFixture.RepositoryMock.Verify(x => x.SearchWithPagination(It.IsAny<QueryFilter<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - SearchWithPaginationAsync - Should return a paginated collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task SearchWithPaginationAsync_ShouldReturnAPaginatedCorrespondingCollectionOfEntities(PaginationResult<Course> paginationResult)
    {
        // Arrange
        var query = _courseFixture.GenerateValidQueryFilter();

        _courseFixture.RepositoryMock
            .Setup(x => x.SearchWithPaginationAsync(It.IsAny<QueryFilter<Course>>()))
            .ReturnsAsync(paginationResult);

        // Act
        var result = await _courseFixture.DomainServiceImpl.SearchWithPaginationAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paginationResult.Page, result.Page);
        Assert.Equal(paginationResult.PageSize, result.PageSize);
        Assert.Equal(paginationResult.TotalRecords, result.TotalRecords);
        Assert.Equal(paginationResult.Result.Count, result.Result?.Count);
        _courseFixture.RepositoryMock.Verify(x => x.SearchWithPaginationAsync(It.IsAny<QueryFilter<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - Update - Should update the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Update_ShouldUpdateTheEntity(Course course)
    {
        // Act
        _courseFixture.DomainServiceImpl.Update(course);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.Update(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateAndCommit - Should update and commit the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateAndCommit_ShouldUpdateAndCommitTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateAndCommit(It.IsAny<Course>()))
            .Returns(course);

        // Act
        var result = _courseFixture.DomainServiceImpl.UpdateAndCommit(course);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        _courseFixture.RepositoryMock.Verify(x => x.UpdateAndCommit(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateAndCommitAsync - Should update and commit the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateAndCommitAsync_ShouldUpdateAndCommitTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateAndCommitAsync(It.IsAny<Course>()))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.DomainServiceImpl.UpdateAndCommitAsync(course);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result.Id);
        _courseFixture.RepositoryMock.Verify(x => x.UpdateAndCommitAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateAsync - Should update the entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateAsync_ShouldUpdateTheEntity(Course course)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.UpdateAsync(course);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateRange - Should update a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateRange_ShouldUpdateACollectionOfEntities(ICollection<Course> courses)
    {
        // Act
        _courseFixture.DomainServiceImpl.UpdateRange(courses);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.UpdateRange(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateRangeAndCommit - Should update and commit a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateRangeAndCommit_ShouldUpdateAndCommitACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateRangeAndCommit(It.IsAny<ICollection<Course>>()))
            .Returns(courses);

        // Act
        var result = _courseFixture.DomainServiceImpl.UpdateRangeAndCommit(courses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.UpdateRangeAndCommit(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateRangeAndCommitAsync - Should update and commit a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateRangeAndCommitAsync_ShouldUpdateAndCommitACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateRangeAndCommitAsync(It.IsAny<ICollection<Course>>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.DomainServiceImpl.UpdateRangeAndCommitAsync(courses);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.RepositoryMock.Verify(x => x.UpdateRangeAndCommitAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseDomainService - UpdateRangeAsync - Should update a collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateRangeAsync_ShouldUpdateACollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.RepositoryMock
            .Setup(x => x.UpdateRangeAsync(It.IsAny<ICollection<Course>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.DomainServiceImpl.UpdateRangeAsync(courses);

        // Assert
        _courseFixture.RepositoryMock.Verify(x => x.UpdateRangeAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }
}
