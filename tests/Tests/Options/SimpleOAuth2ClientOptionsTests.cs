using FluentAssertions;
using FluentValidation.TestHelper;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Options;

public class SimpleOAuth2ClientOptionsTests
{
    [UnitTest]
    [Theory]
    [AutoOptionsData]
    internal void GivenClientCredentialOptionsAreNull_WhenValidatorIsExecuted_ThenValidationErrorForClientCredentialOptionsIsAvailable(
        SimpleOAuth2ClientOptions options,
        SimpleOAuth2ClientOptionsValidator sut)
    {
        // Given
        options.ClientCredentialOptions = null!;

        // When
        TestValidationResult<SimpleOAuth2ClientOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.ClientCredentialOptions)
            .WithErrorMessage("The options for grant type client_credential are not configured");
    }

    [UnitTest]
    [Theory]
    [AutoOptionsData]
    internal void GivenRetryOptionsAreNull_WhenValidatorIsExecuted_ThenValidationErrorForRetryOptionsIsAvailable(
        SimpleOAuth2ClientOptions options,
        SimpleOAuth2ClientOptionsValidator sut)
    {
        // Given
        options.RetryOptions = null!;

        // When
        TestValidationResult<SimpleOAuth2ClientOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.RetryOptions)
            .WithErrorMessage("The options for the retry handling are not configured");
    }

    [UnitTest]
    [Theory]
    [AutoOptionsData]
    internal void GiveSimpleOAuth2ClientOptionsAreValid_WhenValidatorIsExecuted_ThenNoValidationErrorsAreAvailable(
        SimpleOAuth2ClientOptions options,
        SimpleOAuth2ClientOptionsValidator sut)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        TestValidationResult<SimpleOAuth2ClientOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .IsValid
            .Should()
            .BeTrue();
    }
}
