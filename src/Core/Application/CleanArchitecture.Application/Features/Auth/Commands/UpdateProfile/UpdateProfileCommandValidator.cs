using FluentValidation;

namespace CleanArchitecture.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Ad alanı 50 karakterden uzun olamaz.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad alanı 50 karakterden uzun olamaz.");
    }
} 