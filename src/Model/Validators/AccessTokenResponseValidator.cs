using FluentValidation;

namespace SimpleOAuth2Client.AspNetCore.Model.Validators;

/// <summary>
/// Customized validator for AccessTokenResponse.
/// </summary>
/// <remarks>This validator use a FluentValidation validator.</remarks>
internal sealed class AccessTokenResponseValidator : AbstractValidator<AccessTokenResponse>
{
    /// <summary>
    /// The constructor.
    /// </summary>
    /// <remarks>The constructor include all validation rules.</remarks>
    public AccessTokenResponseValidator()
    {
        RuleFor(response => response.AccessToken)
            .NotEmpty()
            .WithMessage("The AccessToken is required. The AccessTokenResponse is invalid.");

        RuleFor(response => response.TokenType)
            .NotEmpty()
            .WithMessage("The TokenType is required. The AccessTokenResponse is invalid.");
    }
}
