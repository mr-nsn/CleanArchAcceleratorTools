using Bogus;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Addresses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Instructors;
using CleanArchAcceleratorTools.Examples.Infrastructure.Data.Context;
using CleanArchAcceleratorTools.Examples.Util.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CleanArchAcceleratorTools.Examples.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(DataContext context)
    {
        if (context.Database.IsRelational())
            context.Database.Migrate();

        if (context.Courses.Any())
            return;        

        await context.Courses.AddRangeAsync(GenerateCourses());
        context.SaveChanges();
    }

    public static List<Course> GenerateCourses(int count = 50, int randomSeed = 8675309)
    {
        Randomizer.Seed = new Random(randomSeed);

        var countryFaker = new Faker<Country>("pt_BR")
            .RuleFor(c => c.Id, f => null)
            .RuleFor(c => c.Code, f => f.Address.CountryCode())
            .RuleFor(c => c.Name, f => f.Address.County())
            .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow);

        var stateFaker = new Faker<State>("pt_BR")
            .RuleFor(s => s.Id, f => null)
            .RuleFor(s => s.Code, f => f.Random.Int().ToString())
            .RuleFor(s => s.Name, f => f.Address.State())
            .RuleFor(s => s.Abbreviation, f => f.Address.StateAbbr())
            .RuleFor(r => r.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(s => s.Country, f => countryFaker.Generate());

        var cityFaker = new Faker<City>("pt_BR")
            .RuleFor(c => c.Id, f => null)
            .RuleFor(c => c.Code, f => f.Random.Int().ToString())
            .RuleFor(c => c.Name, f => f.Address.City())
            .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(c => c.State, f => stateFaker.Generate());

        var addressFaker = new Faker<Address>("pt_BR")
            .RuleFor(a => a.Id, f => null)
            .RuleFor(a => a.StreetAvenue, f => f.Address.StreetAddress())
            .RuleFor(a => a.Number, f => f.Address.BuildingNumber())
            .RuleFor(a => a.Complement, f => f.Lorem.Word())
            .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
            .RuleFor(a => a.City, f => cityFaker.Generate())
            .RuleFor(a => a.CreatedAt, f => DateTime.UtcNow);

        var profileFaker = new Faker<Profile>("pt_BR")
            .RuleFor(p => p.Id, f => null)
            .RuleFor(p => p.Bio, f => f.Lorem.Paragraph())
            .RuleFor(p => p.LinkedInUrl, f => $"https://www.linkedin.com/in/{f.Internet.UserName()}")
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.Address, f => addressFaker.Generate());

        var instructorFaker = new Faker<Instructor>("pt_BR")
            .RuleFor(i => i.Id, f => null)
            .RuleFor(i => i.FullName, f => f.Name.FullName())
            .RuleFor(i => i.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(i => i.Profile, f => profileFaker.Generate());

        var lessonFaker = new Faker<Lesson>("pt_BR")
            .RuleFor(l => l.Id, f => null)
            .RuleFor(l => l.Title, f => f.Commerce.ProductName())
            .RuleFor(l => l.Duration, f => TimeSpan.FromMinutes(f.Random.Int(5, 90)))
            .RuleFor(l => l.CreatedAt, f => DateTime.UtcNow);

        var moduleFaker = new Faker<Module>("pt_BR")
            .RuleFor(m => m.Id, f => null)
            .RuleFor(m => m.Name, f => f.Company.CatchPhrase())
            .RuleFor(m => m.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(m => m.Lessons, (f, m) => lessonFaker.Generate(f.Random.Int(2, 6)));

        var courseFaker = new Faker<Course>("pt_BR")
            .RuleFor(c => c.Id, f => null)
            .RuleFor(c => c.Title, f => f.Company.Bs().ToFirstUpper())
            .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(c => c.Instructor, f => instructorFaker.Generate())
            .RuleFor(c => c.Modules, f => moduleFaker.Generate(f.Random.Int(1, 4)));

        return courseFaker.Generate(count);
    }
}
