using FluentValidation;

namespace Infrastructure.Auth.Configuration;

public class JwtSettingsValidator : AbstractValidator<JwtSettings>
{
    public JwtSettingsValidator()
    {
        RuleFor(x => x.Secret)
            .NotEmpty()
            .MinimumLength(32)
            .WithMessage("JWT Secret must be at least 32 characters long");

        RuleFor(x => x.Issuer)
            .NotEmpty()
            .WithMessage("JWT Issuer must not be empty");

        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithMessage("JWT Audience must not be empty");

        RuleFor(x => x.ExpirationMinutes)
            .GreaterThan(0)
            .WithMessage("JWT ExpirationMinutes must be greater than 0");
    }
}