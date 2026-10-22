using FluentValidation;

namespace Infrastructure.Auth.Configuration;

public class JwtTokensSettingsValidator : AbstractValidator<JwtSettings>
{
    public JwtTokensSettingsValidator()
    {
        RuleFor(x => x.Issuer)
            .NotEmpty()
            .WithMessage("Missing issuer name.");

        RuleFor(x => x.Audience)
            .NotEmpty()
            .WithMessage("Missing audience name.");

        RuleFor(x => x.SecretKey)
            .NotEmpty()
            .WithMessage("Missing jwt signing key.");
    }
}