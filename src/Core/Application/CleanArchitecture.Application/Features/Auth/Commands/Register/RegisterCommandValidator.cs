using FluentValidation;

namespace CleanArchitecture.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("Ad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Ad alanı 50 karakterden uzun olamaz.");

        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("Soyad alanı boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad alanı 50 karakterden uzun olamaz.");

        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email alanı boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email alanı 100 karakterden uzun olamaz.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Şifre 100 karakterden uzun olamaz.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.");

        RuleFor(v => v.ConfirmPassword)
            .NotEmpty().WithMessage("Şifre tekrar alanı boş olamaz.")
            .Equal(v => v.Password).WithMessage("Şifreler eşleşmiyor.");
    }
} 