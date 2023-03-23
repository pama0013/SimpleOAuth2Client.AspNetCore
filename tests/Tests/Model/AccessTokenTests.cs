using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Model;

public class AccessTokenTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenTokenIsExpired_WhenAccessTokenIsCreated_ThenAccessTokenIsNotValid(
        string token,
        string tokenType)
    {
        // Given
        int expiresIn = 0;

        // When
        var accessToken = new AccessToken(token, tokenType, expiresIn);

        // Then
        accessToken
            .Value
            .Should()
            .Be(token);

        accessToken
            .IsValid
            .Should()
            .BeFalse();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenTokenIsNotExpired_WhenAccessTokenIsCreated_ThenAccessTokenIsValid(
        string token,
        string tokenType,
        int expiresIn)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessToken = new AccessToken(token, tokenType, expiresIn);

        // Then
        accessToken
            .Value
            .Should()
            .Be(token);

        accessToken
            .IsValid
            .Should()
            .BeTrue();
    }
}
