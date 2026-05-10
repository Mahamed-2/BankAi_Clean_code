using FluentValidation;
using Bankai.Application.Commands;

namespace Bankai.Application.Validators;

/// <summary>
/// FluentValidation validator for CreateProductCommand.
/// G requirement: Use FluentValidation to validate incoming commands before handling.
/// The ValidationBehavior MediatR pipeline ensures this runs automatically.
/// </summary>
public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or positive.")
            .PrecisionScale(18, 2, true).WithMessage("Price must have at most 2 decimal places.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("A valid CategoryId is required.");
    }
}
