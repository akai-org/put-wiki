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

        RuleFor(x => x.SecretKey)
            .MinimumLength(32)
            .Matches("[a-zA-Z0-9]")
            .WithMessage("Jwt signing key should be at least 32 ASCII characters (bytes) long.");
    }
}