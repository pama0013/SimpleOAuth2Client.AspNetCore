using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
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
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new HttpRequestException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate(loggerMock.Object)
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
    internal async Task GivenHttpRequestExceptionIsThrown_WhenPostAsyncIsCalled_ThenLogMethodWithLogLevelErrorAndCorrectEventIdIsCalledOnce(
        Uri uri,
        StringContent requestContent,
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new HttpRequestException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate(loggerMock.Object)
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(transientErrorHandler);

        // When
        _ = await httpClient.PostAsync(uri, requestContent);

        // Then
        loggerMock.Verify(_ => _.Log(
            LogLevel.Error,
            1002,
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
    }

    [UnitTest]
    [Fact]
    internal void GivenILoggerIsNull_WhenTransientErrorHandlerDelegateIsCreated_ThenArgumentNullExcpetionWithRelatedParameterNameIsThrown()
    {
        // Given
        ILogger<TransientErrorHandlerDelegate> invalidLogger = null!;

        // When
        Action createSut = () => _ = new TransientErrorHandlerDelegate(invalidLogger);

        // Then
        createSut
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("logger");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenILoggerIsValid_WhenTransientErrorHandlerDelegateIsCreated_ThenNoArgumentNullExcpetionWIsThrown(
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action createSut = () => _ = new TransientErrorHandlerDelegate(loggerMock.Object);

        // Then
        createSut
            .Should()
            .NotThrow<ArgumentNullException>();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenInvalidOperationIsThrown_WhenHttpClientIsExecuted_ThenTransientErrorHandlerDelegateRethrowTheException(
        Uri uri,
        StringContent requestContent,
        string exceptionMessage,
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new InvalidOperationException(exceptionMessage));

        using var transientErrorHandler = new TransientErrorHandlerDelegate(loggerMock.Object)
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
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new TaskCanceledException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate(loggerMock.Object)
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
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<TransientErrorHandlerDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Fallback
            .Throw(new TimeoutRejectedException());

        using var transientErrorHandler = new TransientErrorHandlerDelegate(loggerMock.Object)
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
