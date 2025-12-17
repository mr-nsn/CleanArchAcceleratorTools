using CleanArchAcceleratorTools.Examples.Domain.Aggregates.Courses;
using FluentValidation;

namespace CleanArchAcceleratorTools.Domain.Models.Validators;

public class CourseValidator : AbstractValidator<Course>
{
    public CourseValidator()
    {
        InstructorId();
        Title();
    }

    private void InstructorId()
    {
        RuleFor(x => x.InstructorId)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Course)}.{nameof(Course.InstructorId)}";
                return $"The property \"{property}\" cannot be empty";
            });
    }

    private void Title()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(x =>
            {
                var property = $"{nameof(Course)}.{nameof(Course.Title)}";
                return $"The property \"{property}\" cannot be empty";
            });
    }
}
