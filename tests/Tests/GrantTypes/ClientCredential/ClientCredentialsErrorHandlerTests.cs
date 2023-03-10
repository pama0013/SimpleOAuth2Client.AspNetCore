using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.GrantTypes.ClientCredential;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Testdata;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.GrantTypes.ClientCredential;

public class ClientCredentialsErrorHandlerTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpHttpResponseMessageWithHttpStatusCodeBadRequest_WhenHandleAuthorizationServerErrorIsCalled_ThenOAuth2ErrorWithExpectedErrorDescriptionIsReturned(
        string errorMessage,
        string errorDescription,
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage errorHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.BadRequest,
            AuthorizationServerResponses.CreateErrorResponseWithDescription(errorMessage, errorDescription));

        // When
        OAuth2Error oAuth2Error = await sut.HandleAuthorizationServerError(errorHttpResponseMessage);

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest($"{errorMessage}:{errorDescription}");

        oAuth2Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpHttpResponseMessageWithHttpStatusCodeBadRequestAndMissingDescription_WhenHandleAuthorizationServerErrorIsCalled_ThenThenOAuth2ErrorWithExpectedErrorDescriptionIsReturned(
        string errorMessage,
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage errorHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.BadRequest,
            AuthorizationServerResponses.CreateErrorResponseWithoutDescription(errorMessage));

        // When
        OAuth2Error oAuth2Error = await sut.HandleAuthorizationServerError(errorHttpResponseMessage);

        // Then
        OAuth2Error error = OAuth2Errors.AccessTokenRequest($"{errorMessage}:-");

        oAuth2Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenHttpResonseMessageWithSuccessfulHttpStatusCode_WhenHandleAuthorizationServerErrorIsCalled_ThenInvalidOperationExceptionWithRelatedMessageIsThrown(
        string successMessage,
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage successfulHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.OK,
            new StringContent(successMessage));

        // When
        Func<Task<OAuth2Error>> handleAuthorizationServerErrorCalled = () => sut.HandleAuthorizationServerError(successfulHttpResponseMessage);

        // Then
        handleAuthorizationServerErrorCalled
            .Should()
            .ThrowExactlyAsync<InvalidOperationException>()
            .WithMessage($"HTTP-Status code {successfulHttpResponseMessage.StatusCode} is not a valid HTTP-Status code error.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenHttpResponseMessageIsNull_WhenHandleAuthorizationServerErrorIsCalled_ThenArgumentNullExceptionWithRelatedMessageIsThrown(
        ClientCredentialsErrorHandler sut)
    {
        // Given
        HttpResponseMessage invalidHttpResponseMessage = null!;

        // When
        Func<Task<OAuth2Error>> handleAuthorizationServerErrorCalled = () => sut.HandleAuthorizationServerError(invalidHttpResponseMessage);

        // Then
        handleAuthorizationServerErrorCalled
            .Should()
            .ThrowExactlyAsync<ArgumentNullException>()
            .WithParameterName("httpResponseMessage");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpResponseMessageWithHttpStatusCodeBadRequestAndEmptyAuthorizationServerResponse_WhenHandleAuthorizationServerErrorIsCalled_ThenOAuth2ErrorWithExpectedErrorDescriptionIsReturned(
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage errorHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.BadRequest,
            AuthorizationServerResponses.CreateErrorResponseWithNullValue());

        // When
        OAuth2Error oAuth2Error = await sut.HandleAuthorizationServerError(errorHttpResponseMessage);

        // Then

        OAuth2Error error = OAuth2Errors.Unhandled("null");

        oAuth2Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpResponseMessageWithHttpStatusCodeUnauthorized_WhenHandleAuthorizationServerErrorIsCalled_ThenOAuth2ErrorWithErrorDescriptionInvalidClientIsReturned(
        string errorMessage,
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage errorHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.Unauthorized,
            new StringContent(errorMessage));

        // When
        OAuth2Error oAuth2Error = await sut.HandleAuthorizationServerError(errorHttpResponseMessage);

        // Then

        OAuth2Error error = OAuth2Errors.AccessTokenRequest("invalid_client");

        oAuth2Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpResponseMessageWithUnhandledHttpStatusCode_WhenHandleAuthorizationServerErrorIsCalled_ThenOAuth2ErrorWithExpectedErrorDescriptionIsReturned(
        string errorMessage,
        ClientCredentialsErrorHandler sut)
    {
        // Given
        using HttpResponseMessage errorHttpResponseMessage = CreateHttpResponseMessage(
            HttpStatusCode.BadGateway,
            new StringContent(errorMessage));

        // When
        OAuth2Error oAuth2Error = await sut.HandleAuthorizationServerError(errorHttpResponseMessage);

        // Then

        OAuth2Error error = OAuth2Errors.Unhandled(errorMessage);

        oAuth2Error
            .ErrorCode
            .Should()
            .Be(error.ErrorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(error.ErrorDescription);
    }

    private static HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode statusCode, HttpContent httpResponseContent)
    {
        var httpResponseMessage = new HttpResponseMessage(statusCode)
        {
            Content = httpResponseContent,
        };

        return httpResponseMessage;
    }
}
