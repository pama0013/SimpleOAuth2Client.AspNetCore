using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Model;

public class AccessTokenResponseTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenAllDataForTheAccessTokenResponseAreAvailable_WhenAccessTokenResponseIsCreated_ThenAccessTokenResponsePropertiesAreSetAsExpected(
        string accessToken,
        string tokenType,
        int expiresIn,
        string refreshToken,
        string scope)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessTokenResponse = new AccessTokenResponse(accessToken, tokenType, expiresIn, refreshToken, scope);

        // Then
        accessTokenResponse
            .AccessToken
            .Should()
            .Be(accessToken);

        accessTokenResponse
            .ExpiresIn
            .Should()
            .Be(expiresIn);

        accessTokenResponse
            .TokenType
            .Should()
            .Be(tokenType);

        accessTokenResponse
            .RefreshToken
            .Should()
            .Be(refreshToken);

        accessTokenResponse
            .Scope
            .Should()
            .Be(scope);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenAllRequiredDataForAccessTokenResponseAreAvailable_WhenAccessTokenResponseIsCreated_ThenRequiredAccessTokenResponsePropertiesAreSetAsExpected(
        string accessToken,
        string tokenType,
        int expiresIn)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessTokenResponse = new AccessTokenResponse(accessToken, tokenType, expiresIn);

        // Then
        accessTokenResponse
            .AccessToken
            .Should()
            .Be(accessToken);

        accessTokenResponse
            .ExpiresIn
            .Should()
            .Be(expiresIn);

        accessTokenResponse
            .TokenType
            .Should()
            .Be(tokenType);

        accessTokenResponse
            .RefreshToken
            .Should()
            .BeNull();

        accessTokenResponse
            .Scope
            .Should()
            .BeNull();
    }
}
