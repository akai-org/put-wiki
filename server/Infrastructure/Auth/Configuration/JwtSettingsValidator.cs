using FluentValidation;

namespace Infrastructure.Auth.Configuration;

public class JwtSettingsValidator : AbstractValidator<JwtSettings>
{
    public JwtSettingsValidator()
    {
        RuleFor(x => x.Secret)
            .NotEmpty()
            .WithMessage("JWT Secret must not be empty");

        RuleFor(x => x.Issuer)
            .NotEmpty()
            .WithMessage("JWT Issuer must not be empty");

        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithMessage("JWT Audience must not be empty");
    }
}
