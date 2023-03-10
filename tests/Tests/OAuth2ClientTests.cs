using AutoFixture.Xunit2;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests;

public class OAuth2ClientTests
{
    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIAuthorizationGrantIsAvailable_WhenOAuth2ClientIsCreated_ThenNoArgumentNullExceptionIsThrown(
        Mock<IAuthorizationGrant> authorizationGrantMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new OAuth2Client(authorizationGrantMock.Object);

        // Then
        sutCreated
            .Should()
            .NotThrow<ArgumentNullException>();
    }

    [UnitTest]
    [Fact]
    internal void GivenIAuthorizationGrantIsNull_WhenOAuth2ClientIsCreated_ThenArgumentNullExceptionWithRelatedMessageIsThrown()
    {
        // Given
        IAuthorizationGrant authorizationGrant = null!;

        // When
        Action sutCreated = () => _ = new OAuth2Client(authorizationGrant);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("authorizationGrant");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenIAuthorizationGrantProvideValidAccessToken_WhenRequestAccessTokenIsCalled_ThenValidAccessTokenIsReturned(
        AccessToken accessToken,
        [Frozen] Mock<IAuthorizationGrant> authorizationGrantMock,
        OAuth2Client sut)
    {
        // Given
        authorizationGrantMock.Setup(_ => _.Execute()).ReturnsAsync(accessToken);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.RequestAccessToken();

        // Then
        accessTokenResult
            .IsSuccess
            .Should()
            .BeTrue();

        accessTokenResult
            .Value
            .Value
            .Should()
            .Be(accessToken.Value);

        accessTokenResult
            .Value
            .IsValid
            .Should()
            .BeTrue();
    }
}
