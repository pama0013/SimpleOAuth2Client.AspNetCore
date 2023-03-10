using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation.TestHelper;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Options;

public class ClientCredentialOptionsValidatorTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenClientCredentialOptionsAreValid_WhenValidatorIsExecuted_ThenNoValidationErrorsAreAvailable(
        ClientCredentialOptions options,
        ClientCredentialOptionsValidator sut)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        TestValidationResult<ClientCredentialOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .IsValid
            .Should()
            .BeTrue();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenInvalidClientId_WhenValidatorIsExecuted_ThenValidationErrorForClientIdIsAvailable(
        ClientCredentialOptions options,
        ClientCredentialOptionsValidator sut)
    {
        // Given
        options.ClientId = null!;

        // When
        TestValidationResult<ClientCredentialOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.ClientId)
            .WithErrorMessage("The ClientId is null but required for the OAuth2 protocol. Please add a value for ClientId.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenInvalidClientSecret_WhenValidatorIsExecuted_ThenValidationErrorForClientSecretIsAvailable(
        ClientCredentialOptions options,
        ClientCredentialOptionsValidator sut)
    {
        // Given
        options.ClientSecret = null!;

        // When
        TestValidationResult<ClientCredentialOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.ClientSecret)
            .WithErrorMessage("The ClientSecret is null but required for the OAuth2 protocol. Please add a value for ClientSecret.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenInvalidTokenEndpoint_WhenValidatorIsExecuted_ThenValidationErrorForTokenEndpointIsAvailable(
        ClientCredentialOptions options,
        ClientCredentialOptionsValidator sut)
    {
        // Given
        options.TokenEndpoint = null!;

        // When
        TestValidationResult<ClientCredentialOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.TokenEndpoint)
            .WithErrorMessage("The TokenEndpoint is null but required for the OAuth2 protocol. Please add a value for TokenEndpoint.");
    }
}
