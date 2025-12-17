using Xunit;

namespace CleanArchAcceleratorTools.Test.Base.Fixtures.Courses;

[CollectionDefinition(nameof(CourseCollection))]
public class CourseCollection : ICollectionFixture<CourseFixture>
{

}
