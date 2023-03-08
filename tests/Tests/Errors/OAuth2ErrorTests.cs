using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Common.Errors;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Errors;

public class OAuth2ErrorTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    public void GivenErrorCodeandErrorDescriptionAreAvailable_WhenOAuth2ErrorIsCreated_ThenOAuth2ErrorPropertiesAreSetCorrect(
        string errorCode, string errorDescription)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var oAuth2Error = new OAuth2Error(errorCode, errorDescription);

        // Then
        oAuth2Error
            .ErrorCode
            .Should()
            .Be(errorCode);

        oAuth2Error
            .ErrorDescription
            .Should()
            .Be(errorDescription);
    }
}
