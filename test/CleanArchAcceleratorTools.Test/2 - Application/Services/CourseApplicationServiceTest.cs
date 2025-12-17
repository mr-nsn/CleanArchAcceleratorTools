using CleanArchAcceleratorTools.Domain.Models;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;
using CleanArchAcceleratorTools.Test.Customization;
using Mapster;
using Moq;

namespace CleanArchAcceleratorTools.Test.Application.Services;

[Collection(nameof(CourseCollection))]
public class CourseApplicationServiceTest
{
    private readonly CourseFixture _courseFixture;

    public CourseApplicationServiceTest(CourseFixture courseFixture)
    {
        _courseFixture = courseFixture;
        _courseFixture.Reset();
    }

    [Theory(DisplayName = "CourseApplicationService - Add - Should add the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Add_ShouldCallTheDomainAddMethod(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        // Act
        _courseFixture.ApplicationServiceImpl.Add(vm);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.Add(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddAndCommit - Should add and commit the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddAndCommit_ShouldAddAndCommitTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddAndCommit(It.IsAny<Course>()))
            .Returns(course);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.AddAndCommit(vm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vm.Id, result.Id);
        Assert.Equal(vm.InstructorId, result.InstructorId);
        _courseFixture.DomainServiceMock.Verify(x => x.AddAndCommit(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddAndCommitAsync - Should add and commit the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddAndCommitAsync_ShouldAddAndCommitTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddAndCommitAsync(It.IsAny<Course>()))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.AddAndCommitAsync(vm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vm.Id, result.Id);
        _courseFixture.DomainServiceMock.Verify(x => x.AddAndCommitAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddAsync - Should add the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddAsync_ShouldAddTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.AddAsync(vm);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.AddAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddRange - Should add a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddRange_ShouldAddACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        // Act
        _courseFixture.ApplicationServiceImpl.AddRange(vms);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.AddRange(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddRangeAndCommit - Should add and commit a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void AddRangeAndCommit_ShouldAddAndCommitACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddRangeAndCommit(It.IsAny<ICollection<Course>>()))
            .Returns(courses);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.AddRangeAndCommit(vms);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.AddRangeAndCommit(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddRangeAndCommitAsync - Should add and commit a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddRangeAndCommitAsync_ShouldAddAndCommitACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddRangeAndCommitAsync(It.IsAny<ICollection<Course>>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.AddRangeAndCommitAsync(vms);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.AddRangeAndCommitAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - AddRangeAsync - Should add a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task AddRangeAsync_ShouldAddACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.AddRangeAsync(It.IsAny<ICollection<Course>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.AddRangeAsync(vms);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.AddRangeAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - DynamicSelectAsync - Should return a collection of ViewModels")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task DynamicSelectAsync_ShouldReturnACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var dynamicSelect = _courseFixture.GenerateValidDynamicSelectViewModel();

        _courseFixture.DomainServiceMock
            .Setup(x => x.DynamicSelectAsync(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()))
            .ReturnsAsync(courses.ToList());

        // Act
        var result = await _courseFixture.ApplicationServiceImpl
            .DynamicSelectAsync(dynamicSelect.DynamicFilter, dynamicSelect.DynamicSort, dynamicSelect.Fields);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.DynamicSelectAsync(It.IsAny<DynamicFilter<Course>>(), It.IsAny<DynamicSort<Course>>(), It.IsAny<string[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - GetAll - Should return a collection of ViewModels")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void GetAll_ShouldReturnACorrespondingCollectionOfViewModels(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.GetAll())
            .Returns(courses);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - GetAllAsync - Should return a collection of ViewModels")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetAllAsync_ShouldReturnACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - GetById - Should return a ViewModel when found")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void GetById_ShouldReturnAViewModelWhenFound(Course course)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.GetById(It.IsAny<long>()))
            .Returns(course);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.GetById(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result?.Id);
        _courseFixture.DomainServiceMock.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - GetByIdAsync - Should return a ViewModel when found")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task GetByIdAsync_ShouldReturnTheCorrespondingEntityWhenFound(Course course)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.GetByIdAsync(It.IsAny<long>()))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.GetByIdAsync(course.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(course.Id, result?.Id);
        _courseFixture.DomainServiceMock.Verify(x => x.GetByIdAsync(It.IsAny<long>()), Times.Once);
    }

    // R - Remove*
    [Theory(DisplayName = "CourseApplicationService - Remove - Should remove by id")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Remove_ShouldRemoveById(long id)
    {
        // Act
        _courseFixture.ApplicationServiceImpl.Remove(id);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.Remove(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveAndCommit - Should remove by id and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveAndCommit_ShouldRemoveByIdAndCommit(long id)
    {
        // Act
        _courseFixture.ApplicationServiceImpl.RemoveAndCommit(id);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveAndCommit(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveAndCommitAsync - Should remove by id and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveAndCommitAsync_ShouldRemoveByIdAndCommit(long id)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.RemoveAndCommitAsync(It.IsAny<long>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.RemoveAndCommitAsync(id);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveAndCommitAsync(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveAsync - Should remove by id")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveAsync_ShouldRemoveById(long id)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.RemoveAsync(It.IsAny<long>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.RemoveAsync(id);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveAsync(It.IsAny<long>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveRange - Should remove a range of ids")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveRange_ShouldRemoveARangeOfIds(long?[] ids)
    {
        // Act
        _courseFixture.ApplicationServiceImpl.RemoveRange(ids);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveRange(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveRangeAndCommit - Should remove a range of ids and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void RemoveRangeAndCommit_ShouldRemoveARangeOfIdsAndCommit(long?[] ids)
    {
        // Act
        _courseFixture.ApplicationServiceImpl.RemoveRangeAndCommit(ids);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveRangeAndCommit(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveRangeAndCommitAsync - Should remove a range of ids and commit")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveRangeAndCommitAsync_ShouldRemoveARangeOfIdsAndCommit(long?[] ids)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.RemoveRangeAndCommitAsync(It.IsAny<long?[]>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.RemoveRangeAndCommitAsync(ids);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveRangeAndCommitAsync(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - RemoveRangeAsync - Should remove a range of ids")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task RemoveRangeAsync_ShouldRemoveARangeOfIds(long?[] ids)
    {
        // Arrange
        _courseFixture.DomainServiceMock
            .Setup(x => x.RemoveRangeAsync(It.IsAny<long?[]>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.RemoveRangeAsync(ids);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.RemoveRangeAsync(It.IsAny<long?[]>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - SearchWithPagination - Should return a paginated collection of ViewModels")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void SearchWithPagination_ShouldReturnAPaginatedCorrespondingCollectionOfEntities(PaginationResult<Course> paginationResult)
    {
        // Arrange
        var query = _courseFixture.GenerateValidQueryFilterViewModel();

        _courseFixture.DomainServiceMock
            .Setup(x => x.SearchWithPagination(It.IsAny<QueryFilter<Course>>()))
            .Returns(paginationResult);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.SearchWithPagination(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paginationResult.Page, result.Page);
        Assert.Equal(paginationResult.PageSize, result.PageSize);
        Assert.Equal(paginationResult.TotalRecords, result.TotalRecords);
        Assert.Equal(paginationResult.Result.Count, result.Result?.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.SearchWithPagination(It.IsAny<QueryFilter<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - SearchWithPaginationAsync - Should return a paginated collection of ViewModels")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task SearchWithPaginationAsync_ShouldReturnAPaginatedCorrespondingCollectionOfEntities(PaginationResult<Course> paginationResult)
    {
        // Arrange
        var query = _courseFixture.GenerateValidQueryFilterViewModel();

        _courseFixture.DomainServiceMock
            .Setup(x => x.SearchWithPaginationAsync(It.IsAny<QueryFilter<Course>>()))
            .ReturnsAsync(paginationResult);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.SearchWithPaginationAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(paginationResult.Page, result.Page);
        Assert.Equal(paginationResult.PageSize, result.PageSize);
        Assert.Equal(paginationResult.TotalRecords, result.TotalRecords);
        Assert.Equal(paginationResult.Result.Count, result.Result?.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.SearchWithPaginationAsync(It.IsAny<QueryFilter<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - Update - Should update the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void Update_ShouldUpdateTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        // Act
        _courseFixture.ApplicationServiceImpl.Update(vm);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.Update(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateAndCommit - Should update and commit the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateAndCommit_ShouldUpdateAndCommitTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateAndCommit(It.IsAny<Course>()))
            .Returns(course);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.UpdateAndCommit(vm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vm.Id, result.Id);
        Assert.Equal(vm.InstructorId, result.InstructorId);
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateAndCommit(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateAndCommitAsync - Should update and commit the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateAndCommitAsync_ShouldUpdateAndCommitTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateAndCommitAsync(It.IsAny<Course>()))
            .ReturnsAsync(course);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.UpdateAndCommitAsync(vm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(vm.Id, result.Id);
        Assert.Equal(vm.InstructorId, result.InstructorId);
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateAndCommitAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateAsync - Should update the corresponding entity")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateAsync_ShouldUpdateTheCorrespondingEntity(Course course)
    {
        // Arrange
        var vm = course.Adapt<CourseViewModel>();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateAsync(It.IsAny<Course>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.UpdateAsync(vm);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateAsync(It.IsAny<Course>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateRange - Should update a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateRange_ShouldUpdateACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        // Act
        _courseFixture.ApplicationServiceImpl.UpdateRange(vms);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateRange(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateRangeAndCommit - Should update and commit a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public void UpdateRangeAndCommit_ShouldUpdateAndCommitACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateRangeAndCommit(It.IsAny<ICollection<Course>>()))
            .Returns(courses);

        // Act
        var result = _courseFixture.ApplicationServiceImpl.UpdateRangeAndCommit(vms);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateRangeAndCommit(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateRangeAndCommitAsync - Should update and commit a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateRangeAndCommitAsync_ShouldUpdateAndCommitACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateRangeAndCommitAsync(It.IsAny<ICollection<Course>>()))
            .ReturnsAsync(courses);

        // Act
        var result = await _courseFixture.ApplicationServiceImpl.UpdateRangeAndCommitAsync(vms);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courses.Count, result.Count);
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateRangeAndCommitAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }

    [Theory(DisplayName = "CourseApplicationService - UpdateRangeAsync - Should update a correponding collection of entities")]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    [InlineAutoDataCustom()]
    public async Task UpdateRangeAsync_ShouldUpdateACorrespondingCollectionOfEntities(ICollection<Course> courses)
    {
        // Arrange
        var vms = courses.Select(c => c.Adapt<CourseViewModel>()).ToList();

        _courseFixture.DomainServiceMock
            .Setup(x => x.UpdateRangeAsync(It.IsAny<ICollection<Course>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _courseFixture.ApplicationServiceImpl.UpdateRangeAsync(vms);

        // Assert
        _courseFixture.DomainServiceMock.Verify(x => x.UpdateRangeAsync(It.IsAny<ICollection<Course>>()), Times.Once);
    }
}
