using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Addresses;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings.Aggregates.Instructors;
using CleanArchAcceleratorTools.Domain.Interfaces;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data.Mappings;

public static class MappingsHolder
{
    public static Dictionary<Type, IEntityTypeConfiguration> GetMappings()
    {
        var mappings = new Dictionary<Type, IEntityTypeConfiguration>();

        mappings.Add(typeof(Course), new CourseMap());
        mappings.Add(typeof(Lesson), new LessonMap());
        mappings.Add(typeof(Module), new ModuleMap());
        mappings.Add(typeof(Instructor), new InstructorMap());
        mappings.Add(typeof(Profile), new ProfileMap());
        mappings.Add(typeof(Address), new AddressMap());
        mappings.Add(typeof(City), new CityMap());
        mappings.Add(typeof(State), new StateMap());
        mappings.Add(typeof(Country), new CountryMap());

        return mappings;
    }
}
