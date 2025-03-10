using FluentValidation;
using Library.Services.DTOs;

namespace Library.Services.Validators
{
    public class BookDtoValidator : AbstractValidator<BookDto>
    {
        public BookDtoValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author name cannot exceed 100 characters.");

            RuleFor(x => x.PublicationYear)
                .NotEmpty().WithMessage("Publication year is required.")
                .InclusiveBetween(1800, DateTime.UtcNow.Year)
                .WithMessage("Publication year must be between 1800 and the current year.");

            RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.")
                .Matches(@"^(?:\d{13}|\d{10})$").WithMessage("ISBN must be 10 or 13 digits.");
        }
    }
}