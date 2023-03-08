using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Errors;

public class OAuth2ErrorsTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    public void GivenErrorDescriptionIsAvailable_WhenAccessTokenRequestIsCalled_ThenExpectedOAuth2ErrorIsReturned(
        string errorDescription)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        OAuth2Error oAuth2Error = OAuth2Errors.AccessTokenRequest(errorDescription);

        // Then
        oAuth2Error
            .ErrorCode
            .Should()
            .Be("OAuth2.AccessTokenRequest");

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(errorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    public void GivenErrorDescriptionIsAvailable_WhenAccessTokenResponseIsCalled_ThenExpectedOAuth2ErrorIsReturned(
    string errorDescription)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        OAuth2Error oAuth2Error = OAuth2Errors.AccessTokenResponse(errorDescription);

        // Then
        oAuth2Error
            .ErrorCode
            .Should()
            .Be("OAuth2.AccessTokenResponse");

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(errorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    public void GivenErrorDescriptionIsAvailable_WhenUnhandledIsCalled_ThenExpectedOAuth2ErrorIsReturned(
        string errorDescription)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        OAuth2Error oAuth2Error = OAuth2Errors.Unhandled(errorDescription);

        // Then
        oAuth2Error
            .ErrorCode
            .Should()
            .Be("OAuth2.Unhandled");

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(errorDescription);
    }
}
