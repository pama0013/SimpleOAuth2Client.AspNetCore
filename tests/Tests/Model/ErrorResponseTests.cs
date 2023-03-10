using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Model;

public class ErrorResponseTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenDataForErrorResponseAreAvailable_WhenErrorResponseIsCreated_ThenErrorResponsePropertiesAreSetAsExpected(
        string error,
        string errorDescription,
        Uri errorUri)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var errorResponse = new ErrorResponse(error, errorDescription, errorUri);

        // Then
        errorResponse
            .Error
            .Should()
            .Be(error);

        errorResponse
            .ErrorDescription
            .Should()
            .Be(errorDescription);

        errorResponse
            .ErrorUri
            .Should()
            .Be(errorUri);
    }
}
