using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Polly.Timeout;
using RichardSzalay.MockHttp;
using SimpleOAuth2Client.AspNetCore.Common.Http.Delegates;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Http;

public class TransientErrorHandlerDelegateTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpRequestExceptionIsThrown_WhenHttpClientIsExecuted_ThenTransientErrorHandlerDelegateReturnHttpStatusCode500InternalServerError(
        Uri uri,
        StringContent requestContent,
        MockHttpMessageHandler httpMessageHandlerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new HttpRequestException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(transientErrorHandler);

        // When
        HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, requestContent);

        // Then
        responseMessage
            .StatusCode
            .Should()
            .Be(HttpStatusCode.InternalServerError);

        string httpResponseMessageContent = await responseMessage.Content.ReadAsStringAsync();

        httpResponseMessageContent
            .Should()
            .Be("The authorization server is currently not available.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenInvalidOperationIsThrown_WhenHttpClientIsExecuted_ThenTransientErrorHandlerDelegateRethrowTheException(
        Uri uri,
        StringContent requestContent,
        string exceptionMessage,
        MockHttpMessageHandler httpMessageHandlerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new InvalidOperationException(exceptionMessage));

        using var transientErrorHandler = new TransientErrorHandlerDelegate
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(transientErrorHandler);

        // When
        Func<Task> postAsyncCalled = async () => _ = await httpClient.PostAsync(uri, requestContent);

        // Then
        postAsyncCalled
            .Should()
            .ThrowExactlyAsync<InvalidOperationException>()
            .WithMessage(exceptionMessage);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenTaskCanceledExceptionIsThrown_WhenHttpClientIsExecuted_ThenTransientErrorHandlerDelegateReturnHttpStatusCode500InternalServerError(
        Uri uri,
        StringContent requestContent,
        MockHttpMessageHandler httpMessageHandlerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new TaskCanceledException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(transientErrorHandler);

        // When
        HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, requestContent);

        // Then
        responseMessage
            .StatusCode
            .Should()
            .Be(HttpStatusCode.InternalServerError);

        string httpResponseMessageContent = await responseMessage.Content.ReadAsStringAsync();

        httpResponseMessageContent
            .Should()
            .Be("The authorization server is currently not available.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenTimeoutRejectedExceptionIsThrown_WhenHttpClientIsExecuted_ThenTransientErrorHandlerDelegateReturnHttpStatusCode500InternalServerError(
        Uri uri,
        StringContent requestContent,
        MockHttpMessageHandler httpMessageHandlerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new TimeoutRejectedException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(transientErrorHandler);

        // When
        HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, requestContent);

        // Then
        responseMessage
            .StatusCode
            .Should()
            .Be(HttpStatusCode.InternalServerError);

        string httpResponseMessageContent = await responseMessage.Content.ReadAsStringAsync();

        httpResponseMessageContent
            .Should()
            .Be("The authorization server is currently not available.");
    }
}
