using System.Net;
using AutoFixture.Xunit2;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
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
        Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            httpClientFactoryMock.Object,
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
        string accessToken,
        string tokenType,
        int expiresIn,
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        using HttpContent accessTokenResponse = AuthorizationServerResponses.CreateAccessTokenResponse(accessToken, tokenType, expiresIn);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
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
            .Be(accessToken);

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
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        using HttpContent accessTokenResponse = AuthorizationServerResponses.CreateAccessTokenResponseWithNullValue();

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.OK, accessTokenResponse);

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
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeBadRequestWithErrorMessage_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        string errorMessage,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);
        using HttpContent httpResponseContent = AuthorizationServerResponses.CreateErrorResponseWithoutDescription(errorMessage);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.BadRequest, httpResponseContent);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest($"[{errorMessage}|-]");

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeBadRequestWithErrorMessageAndErrorDescription_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        string errorMessage,
        string errorDescription,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);
        using HttpContent httpResponseContent = AuthorizationServerResponses.CreateErrorResponseWithDescription(errorMessage, errorDescription);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.BadRequest, httpResponseContent);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest($"[{errorMessage}|{errorDescription}]");

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeBadRequestWithNullValue_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);
        using HttpContent httpResponseContent = AuthorizationServerResponses.CreateAccessTokenResponseWithNullValue();

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.BadRequest, httpResponseContent);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest("null");

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeInternalServerError_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        string errorMessage,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);
        using HttpContent accessTokenResponse = AuthorizationServerResponses.CreateTransientErrorResponse(errorMessage);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.InternalServerError, accessTokenResponse);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.AuthorizationServer(errorMessage);

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeThatIsNotHandled_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        string errorMessage,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);
        using HttpContent accessTokenResponse = AuthorizationServerResponses.CreateTransientErrorResponse(errorMessage);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.BadGateway, accessTokenResponse);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.Unhandled(errorMessage);

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal async Task GivenAuthorizationServerReturnsHttpStatusCodeUnauthorized_WhenExecuteIsCalled_ThenRelatedOAuth2ErrorIsReturned(
        [Frozen] Mock<IHttpClientFactory> httpClientFactoryMock,
        [Frozen] Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock,
        MockHttpMessageHandler httpMessageHandlerMock,
        SimpleOAuth2ClientOptions options,
        ClientCredentials sut)
    {
        // Given
        using var httpClient = new HttpClient(httpMessageHandlerMock);

        SetupHttpClientFactory(httpClientFactoryMock, httpClient);
        SetupSimpleOAuth2ClientOptions(optionsMonitorMock, options);

        httpMessageHandlerMock
            .When(HttpMethod.Post, options.ClientCredentialOptions.TokenEndpoint.ToString())
            .Respond(HttpStatusCode.Unauthorized);

        // When
        Result<AccessToken, OAuth2Error> accessTokenResult = await sut.Execute();

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest("invalid_client");

        accessTokenResult
            .Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        accessTokenResult
            .Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIHttpClientFactoryIsNull_WhenClientCredentialsIsCreated_ThenArgumentNullExcpetionWithRelatedErrorIsThrown(
        Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock)
    {
        // Given
        IHttpClientFactory invalidHttpClientFactory = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            invalidHttpClientFactory,
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
        Mock<IHttpClientFactory> httpClientFactoryMock)
    {
        // Given
        IOptionsMonitor<SimpleOAuth2ClientOptions> invalidOptionsMonitor = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentials(
            httpClientFactoryMock.Object,
            invalidOptionsMonitor);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("options");
    }

    private static void SetupHttpClientFactory(Mock<IHttpClientFactory> httpClientFactoryMock, HttpClient httpClient)
    {
        httpClientFactoryMock
            .Setup(_ => _.CreateClient("AuthClient"))
            .Returns(httpClient);
    }

    private static void SetupSimpleOAuth2ClientOptions(Mock<IOptionsMonitor<SimpleOAuth2ClientOptions>> optionsMonitorMock, SimpleOAuth2ClientOptions options)
    {
        optionsMonitorMock
            .SetupGet(_ => _.CurrentValue)
            .Returns(options);
    }
}
