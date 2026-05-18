using System;

using FluentValidation;

namespace Application.Auth;

public class UsosOAuthSettingsValidator : AbstractValidator<UsosOAuthSettings>
{
    public UsosOAuthSettingsValidator()
    {
        RuleFor(x => x.ConsumerKey)
            .NotEmpty()
            .WithMessage("Missing UsosOAuth consumer key.");

        RuleFor(x => x.ConsumerSecret)
            .NotEmpty()
            .WithMessage("Missing UsosOAuth consumer secret.");

        RuleFor(x => x.BaseUrl)
            .NotEmpty()
            .WithMessage("Missing UsosOAuth base URL.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out var outUri)
                         && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("UsosOAuth base URL must be an absolute http(s) URL).");

        RuleFor(x => x.CallbackUrl)
            .NotEmpty()
            .WithMessage("Missing UsosOAuth callback URL.");

        RuleFor(x => x.Scopes)
            .NotEmpty()
            .WithMessage("Missing UsosOAuth scopes.");

        RuleFor(x => x.HashingKey)
            .NotEmpty()
            .WithMessage("Missing user's usos id hashing key.");
    }
}