using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Common.Http;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Http;

public class BasicAuthenticationHeaderValueTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenUsernameAndPasswordAreAvailable_WhenBasicAuthenticationHeaderValueIsCreated_ThenSchemeAndParameterAreSetAsExpected(
        string username,
        string password)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var basicAuthenticationHeaderValue = new BasicAuthenticationHeaderValue(username, password);

        // Then
        basicAuthenticationHeaderValue
            .Scheme
            .Should()
            .Be("Basic");

        basicAuthenticationHeaderValue
            .Parameter
            .Should()
            .NotBeNullOrEmpty();
    }
}
