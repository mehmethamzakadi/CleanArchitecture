using FluentValidation;

namespace CleanArchitecture.Application.Features.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email alanı boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Şifre alanı boş olamaz.");
    }
} 