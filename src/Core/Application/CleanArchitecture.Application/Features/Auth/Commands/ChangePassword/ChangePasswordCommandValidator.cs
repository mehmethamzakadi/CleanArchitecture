using FluentValidation;

namespace CleanArchitecture.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(v => v.CurrentPassword)
            .NotEmpty().WithMessage("Mevcut şifre alanı boş olamaz.");

        RuleFor(v => v.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre alanı boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Şifre 100 karakterden uzun olamaz.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.")
            .NotEqual(v => v.CurrentPassword).WithMessage("Yeni şifre mevcut şifre ile aynı olamaz.");
    }
} 