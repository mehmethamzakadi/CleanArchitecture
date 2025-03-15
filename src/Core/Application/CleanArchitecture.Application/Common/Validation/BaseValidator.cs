using FluentValidation;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Application.Common.Validation
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        protected void ValidateEmail(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");
        }

        protected void ValidatePassword(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Must(password => !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[A-Z]")).WithMessage("Password must contain at least one uppercase letter.")
                .Must(password => !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[a-z]")).WithMessage("Password must contain at least one lowercase letter.")
                .Must(password => !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[0-9]")).WithMessage("Password must contain at least one number.")
                .Must(password => !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[^a-zA-Z0-9]")).WithMessage("Password must contain at least one special character.");
        }

        protected void ValidateRequired<TProperty>(System.Linq.Expressions.Expression<Func<T, TProperty>> expression, string propertyName, int? minLength = null, int? maxLength = null)
        {
            var rule = RuleFor(expression)
                .NotEmpty().WithMessage($"{propertyName} is required.");

            if (minLength.HasValue && typeof(TProperty) == typeof(string))
            {
                rule.Must(value => value != null && value.ToString()!.Length >= minLength.Value)
                    .WithMessage($"{propertyName} must be at least {minLength.Value} characters.");
            }

            if (maxLength.HasValue && typeof(TProperty) == typeof(string))
            {
                rule.Must(value => value != null && value.ToString()!.Length <= maxLength.Value)
                    .WithMessage($"{propertyName} must not exceed {maxLength.Value} characters.");
            }
        }

        protected void ValidatePhone(System.Linq.Expressions.Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("Phone number is required.")
                .Must(phone => !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, @"^\+?[1-9]\d{1,14}$"))
                .WithMessage("Phone number must be a valid phone number in E.164 format.");
        }
    }
} 