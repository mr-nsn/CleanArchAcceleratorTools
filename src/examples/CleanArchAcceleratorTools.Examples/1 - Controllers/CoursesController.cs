using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Controller;
using CleanArchAcceleratorTools.Domain.Interfaces;
using CleanArchAcceleratorTools.Domain.Mediator;
using CleanArchAcceleratorTools.Domain.Mediator.DomainNotification.Event;
using CleanArchAcceleratorTools.Examples.Application.Aggregates.Courses.Services;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Applications.ViewModels.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CleanArchAcceleratorTools.Examples.Controllers;

[ApiController]
[Route("courses")]
public class CoursesController : GenericController<Course, CourseViewModel>
{
    private readonly ICourseApplicationService _courseApplicationService;

    public CoursesController(
        ICleanArchMediator mediator,
        INotificationHandler<DomainNotificationEvent> notificationHandler,
        IApplicationLogger applicationLogger,
        ICourseApplicationService courseApplicationService
    ) : base(courseApplicationService, mediator, notificationHandler, applicationLogger)
    {
        _courseApplicationService = courseApplicationService;
    }

    #region Get and Find

    [HttpPost("search-with-pagination")]
    public async Task<IActionResult> SearchWithPaginationAsync(QueryFilterViewModel<CourseViewModel> queryFilterViewModel)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.SearchWithPaginationAsync(queryFilterViewModel);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpPost("dynamic-select")]
    public async Task<IActionResult> DynamicSelectAsync(CourseDinamicSelectViewModel courseDynamicSelect)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.DynamicSelectAsync(courseDynamicSelect.DynamicFilter, courseDynamicSelect.DynamicSort, courseDynamicSelect.Fields);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.GetAllAsync();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpGet("all-parallel")]
    public async Task<IActionResult> GetAllParallelAsync()
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.GetAllParallelAsync();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetByIdAsync(long? id)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.GetByIdAsync(id);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    #endregion

    #region Add

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync(CourseViewModel course)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.AddAndCommitAsync(course);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpPost("add-range")]
    public async Task<IActionResult> AddRangeAsync(ICollection<CourseViewModel> courses)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.AddRangeAndCommitAsync(courses);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpPost("instructors/add-procedure")]
    public async Task<IActionResult> AddInstructorsProcedureAsync(ICollection<InstructorViewModel> instructors)
    {
        var timer = new Stopwatch();
        timer.Start();

        await _courseApplicationService.AddInstructorsProcedureAsync(instructors);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(null);
    }

    #endregion

    #region Update

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(CourseViewModel course)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.UpdateAndCommitAsync(course);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    [HttpPut("update-range")]
    public async Task<IActionResult> UpdateRangeAsync(ICollection<CourseViewModel> courses)
    {
        var timer = new Stopwatch();
        timer.Start();

        var result = await _courseApplicationService.UpdateRangeAndCommitAsync(courses);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(result);
    }

    #endregion

    #region Remove

    [HttpDelete("delete")]
    public async Task<IActionResult> RemoveAsync(long? id)
    {
        var timer = new Stopwatch();
        timer.Start();

        await _courseApplicationService.RemoveAndCommitAsync(id);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(null);
    }

    [HttpDelete("delete-range")]
    public async Task<IActionResult> RemoveRangeAsync(long?[] ids)
    {
        var timer = new Stopwatch();
        timer.Start();

        await _courseApplicationService.RemoveRangeAndCommitAsync(ids);

        timer.Stop();
        var timeTaken = timer.Elapsed;

        await RaiseNotificationAsync(
            DomainNotificationType.Information,
            Guid.NewGuid().ToString(),
            $"Total execution time: {timeTaken.ToString(@"hh\:mm\:ss\.fff")}"
        );

        return ResponseResult(null);
    }

    #endregion
}
