using CleanArchAcceleratorTools.Application.ViewModels;
using CleanArchAcceleratorTools.Examples.Application.ViewModels.Aggregates.Courses;

namespace CleanArchAcceleratorTools.Examples.Applications.ViewModels.Aggregates.Courses
{
    public class CourseDinamicSelectViewModel
    {
        public string[] Fields { get; set; } = Array.Empty<string>();
        public DynamicFilterViewModel<CourseViewModel>? DynamicFilter { get; set; }
        public DynamicSortViewModel<CourseViewModel>? DynamicSort { get; set; }
    }
}
