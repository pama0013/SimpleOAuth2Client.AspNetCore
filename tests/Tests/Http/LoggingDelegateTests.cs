using System.Net;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using SimpleOAuth2Client.AspNetCore.Common.Http.Delegates;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Http;

public class LoggingDelegateTests
{
    private const int HttpRequestMessageEventId = 1000;
    private const int HttpResponseMessageEventId = 1001;

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenAllArgumentsAreValid_WhenLoggingDelegateIsCreated_ThenNoArgumentNullExceptionIsThrown(
        Mock<ILogger<LoggingDelegate>> loggerMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new LoggingDelegate(loggerMock.Object);

        // Then
        sutCreated
            .Should()
            .NotThrow<ArgumentNullException>();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpRequestAndResponseMessageHasContent_WhenPostAsyncIsCalled_ThenLogMethodForHttpRequestMessageIsCalledTwoTimesAndLogMethodForHttpResponseMessageeIsCalledTwoTimes(
        Uri uri,
        StringContent requestContent,
        StringContent responseContent,
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<LoggingDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Expect(HttpMethod.Post, uri.ToString())
            .Respond(HttpStatusCode.OK, responseContent);

        using var loggingDelegate = new LoggingDelegate(loggerMock.Object)
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(loggingDelegate);

        // When
        _ = await httpClient.PostAsync(uri, requestContent);

        // Then
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpRequestMessageEventId, Times.Exactly(2));
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpResponseMessageEventId, Times.Exactly(2));
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpRequestMessageHasNoContent_WhenSendAsyncIsCalled_ThenLogMethodForHttpRequestMessageIsCalledOnceAndLogMethodForHttpResponseMessageIsCalledTwoTimes(
        Uri uri,
        StringContent requestContent,
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<LoggingDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Expect(HttpMethod.Post, uri.ToString())
            .Respond(HttpStatusCode.OK, requestContent);

        using var loggingDelegate = new LoggingDelegate(loggerMock.Object)
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(loggingDelegate);

        using var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri.ToString());

        // When
        _ = await httpClient.SendAsync(httpRequestMessage);

        // Then
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpRequestMessageEventId, Times.Exactly(1));
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpResponseMessageEventId, Times.Exactly(2));
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenHttpResponseMessageHasNoContent_WhenPostAsyncIsCalled_ThenLogMethodForHttpRequestMessageIsCalledTwoTimesAndLogMethodForHttpResponseMessageIsCalledOnce(
        Uri uri,
        StringContent content,
        MockHttpMessageHandler httpMessageHandlerMock,
        Mock<ILogger<LoggingDelegate>> loggerMock)
    {
        // Given
        httpMessageHandlerMock
            .Expect(HttpMethod.Post, uri.ToString())
            .Respond(req => new HttpResponseMessage());

        using var loggingDelegate = new LoggingDelegate(loggerMock.Object)
        {
            InnerHandler = httpMessageHandlerMock
        };

        using var httpClient = new HttpClient(loggingDelegate);

        // When
        _ = await httpClient.PostAsync(uri, content);

        // Then
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpRequestMessageEventId, Times.Exactly(2));
        VerifyLogMethod(loggerMock, LogLevel.Information, HttpResponseMessageEventId, Times.Exactly(1));
    }

    [UnitTest]
    [Fact]
    internal void GivenILoggerIsNull_WhenLoggingDelegateIsCreated_ThenArgumentNullExceptionWithCorrectrParameterIsThrown()
    {
        // Given
        ILogger<LoggingDelegate> invalidLogger = null!;

        // When
        Action sutCreated = () => _ = new LoggingDelegate(invalidLogger);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("logger");
    }

    private static void VerifyLogMethod(Mock<ILogger<LoggingDelegate>> loggerMock, LogLevel logLevel, EventId eventId, Times times)
    {
        loggerMock.Verify(_ => _.Log(
            logLevel,
            eventId,
            It.Is<It.IsAnyType>((v, t) => true),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), times);
    }
}
