using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.Model.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Model.Validators;

public class AccessTokenResponseValidatorTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenAccessTokenIsNullOrEmpty_WhenValidatorIsExecuted_ThenValidationErrorForAccessTokenIsAvailable(
    AccessTokenResponse accessTokenResponse,
    AccessTokenResponseValidator sut)
    {
        // Given
        var invalidAccessTokenResponse = new AccessTokenResponse(null!, accessTokenResponse.TokenType, accessTokenResponse.ExpiresIn);

        // When
        TestValidationResult<AccessTokenResponse> validationResults = sut.TestValidate(invalidAccessTokenResponse);

        // Then
        validationResults
          .ShouldHaveValidationErrorFor(_ => _.AccessToken)
          .WithErrorMessage("The AccessToken is required. The AccessTokenResponse is invalid.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenAccessTokenResponseIsValid_WhenValidatorIsExecuted_ThenNoValidationErrorsAreAvailablen(
        AccessTokenResponse accessTokenResponse,
        AccessTokenResponseValidator sut)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        TestValidationResult<AccessTokenResponse> validationResults = sut.TestValidate(accessTokenResponse);

        // Then
        validationResults.ShouldNotHaveAnyValidationErrors();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenTokenTypeIsNullOrEmpty_WhenValidatorIsExecuted_ThenValidationErrorForTokenTypeIsAvailable(
        AccessTokenResponse accessTokenResponse,
        AccessTokenResponseValidator sut)
    {
        // Given
        var invalidAccessTokenResponse = new AccessTokenResponse(accessTokenResponse.AccessToken, null!, accessTokenResponse.ExpiresIn);

        // When
        TestValidationResult<AccessTokenResponse> validationResults = sut.TestValidate(invalidAccessTokenResponse);

        // Then
        validationResults
          .ShouldHaveValidationErrorFor(_ => _.TokenType)
          .WithErrorMessage("The TokenType is required. The AccessTokenResponse is invalid.");
    }
}
