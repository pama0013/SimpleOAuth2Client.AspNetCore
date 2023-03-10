using System.Net;
using AutoFixture.Xunit2;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Testdata;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.GrantTypes.ClientCredential;

public class ClientCredentialsTests
{
    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenAllDependenciesAreValid_WhenClientCredentialsIsCreated_ThenNoArgumentNullExcpetionShouldThrow(
        Mock<IHttpClientFactory> httpClientFactoryMock,
        Mock<IAuthorizationServerErrorHandler> authorizationServerErrorHandlerMock,
        Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            httpClientFactoryMock.Object,
            authorizationServerErrorHandlerMock.Object,
            optionsMonitorMock.Object);

        // Then
        sutCreated
            .Should()
            .NotThrow<ArgumentNullException>();
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnAccessTokenRepsoneAndHttpStatusCodeOk_WhenExecuteIsCalled_ThenValidAccessTokenIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        ClientCredentialOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupClientCredentialOptions(optionsMonitorMock, options);

        using HttpContent accessTokenResponse = AuthorizationServerResponses.CreateAccessTokenResponse("ABCD", "bearer", 1000);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.OK, accessTokenResponse);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        accessTokenResult
            .IsSuccess
            .Should()
            .BeTrue();

        accessTokenResult
            .Value
            .Value
            .Should()
            .Be("ABCD");

        accessTokenResult
            .Value
            .IsValid
            .Should()
            .BeTrue();
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnInvalidAccessTokenRepsoneAndHttpStatusCodeOk_WhenExecuteIsCalled_ThenOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        ClientCredentialOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupClientCredentialOptions(optionsMonitorMock, options);

        using HttpContent invalidContent = AuthorizationServerResponses.CreateAccessTokenResponseWithNullValue();

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.OK, invalidContent);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        accessTokenResult
            .IsFailure
            .Should()
            .BeTrue();

        OAuth2Error oAuth2Error = OAuth2Errors.AccessTokenResponse("Could not deserialize AccessTokenResponse");

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(oAuth2Error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(oAuth2Error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeBadRequest_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IAuthorizationServerErrorHandler> authorizationServerErrorHandlerMock,
        [Frozen] Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        ClientCredentialOptions options,
        OAuth2Error oAuth2Error,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupClientCredentialOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.BadRequest);

        authorizationServerErrorHandlerMock
            .Setup(_ => _.HandleAuthorizationServerError(It.IsAny<HttpResponseMessage>()))
            .ReturnsAsync(oAuth2Error);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        accessTokenResult
            .IsFailure
            .Should()
            .BeTrue();
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIAuthorizationServerErrorHandlerIsNull_WhenClientCredentialsIsCreated_ThenArgumentNullExceptionWithRelatedMessageIsThrown(
        Mock<IHttpClientFactory> httpClientFactoryMock,
        Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock)
    {
        // Given
        IAuthorizationServerErrorHandler invalidAuthorizationServerErrorHandler = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            httpClientFactoryMock.Object,
            invalidAuthorizationServerErrorHandler,
            optionsMonitorMock.Object);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("authorizationServerErrorHandler");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIHttpClientFactoryIsNull_WhenClientCredentialsIsCreated_ThenArgumentNullExcpetionWithRelatedErrorIsThrown(
        Mock<IAuthorizationServerErrorHandler> authorizationServerErrorHandlerMock,
        Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock)
    {
        // Given
        IHttpClientFactory invalidHttpClientFactory = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            invalidHttpClientFactory,
            authorizationServerErrorHandlerMock.Object,
            optionsMonitorMock.Object);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("httpClientFactory");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIOptionsMonitorIsNull_WhenClientCredentialsIsCreated_ThenArgumentNullExceptionWithRelatedMessageIsThrown(
        Mock<IHttpClientFactory> httpClientFactoryMock,
        Mock<IAuthorizationServerErrorHandler> authorizationServerErrorHandlerMock)
    {
        // Given
        IOptionsMonitor<ClientCredentialOptions> invalidOptionsMonitor = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            httpClientFactoryMock.Object,
            authorizationServerErrorHandlerMock.Object,
            invalidOptionsMonitor);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("options");
    }

    private static void SetupClientCredentialOptions(Mock<IOptionsMonitor<ClientCredentialOptions>> optionsMonitorMock, ClientCredentialOptions options)
    {
        optionsMonitorMock
            .SetupGet(_ => _.CurrentValue)
            .Returns(options);
    }

    private static void SetupHttpClientFactory(Mock<IHttpClientFactory> httpClientFactoryMock, HttpClient httpClient)
    {
        httpClientFactoryMock
            .Setup(_ => _.CreateClient("AuthClient"))
            .Returns(httpClient);
    }
}
