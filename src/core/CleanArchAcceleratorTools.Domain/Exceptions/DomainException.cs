using FluentValidation.Results;

namespace CleanArchAcceleratorTools.Domain.Exceptions;

/// <summary>
/// Exception type for domain validation errors, carrying a <see cref="ValidationResult"/>.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// The validation result associated with the exception.
    /// </summary>
    public ValidationResult ValidationResult { get; private set; }

    /// <summary>
    /// Initializes a new instance with an empty <see cref="ValidationResult"/>.
    /// </summary>
    public DomainException()
    {
        ValidationResult = new ValidationResult();
    }

    /// <summary>
    /// Initializes a new instance with a message and an empty <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="message">Error message.</param>
    public DomainException(string message) : base(message)
    {
        ValidationResult = new ValidationResult();
    }

    /// <summary>
    /// Initializes a new instance with a message, inner exception, and an empty <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="message">Error message.</param>
    /// <param name="innerException">Inner exception.</param>
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
        ValidationResult = new ValidationResult();
    }

    /// <summary>
    /// Initializes a new instance from a <see cref="ValidationResult"/>; message is built from error messages.
    /// </summary>
    /// <param name="validationResult">Validation result with errors.</param>
    public DomainException(ValidationResult validationResult) : base(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)))
    {
        ValidationResult = validationResult;
    }
}
