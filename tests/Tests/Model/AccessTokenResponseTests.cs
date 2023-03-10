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
    internal void GivenDataForAccessTokenAreAvailable_WhenAccessTokenResponseIsCreated_ThenAccessTokenResponsePropertiesAreSetAsExpected(
        string accessToken,
        int expiresIn,
        string tokenType,
        string refreshToken,
        string scope)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessTokenResponse = new AccessTokenResponse(accessToken, expiresIn, tokenType, refreshToken, scope);

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
}
