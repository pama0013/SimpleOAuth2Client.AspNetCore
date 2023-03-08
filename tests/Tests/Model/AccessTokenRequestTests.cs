using AutoFixture.Xunit2;
using FluentAssertions;
using SimpleOAuth2Client.AspNetCore.Model;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Model;

public class AccessTokenRequestTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenGrantTypeAndScopeAreAvailable_WhenAccessTokenRequestIsCreated_ThenHttpContentWithGrantTypeAndScopeIsUsed(
        string grantType)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessTokenRequest = new AccessTokenRequest(grantType);

        // Then

        string expectedHttpConent = await CreateFormUrlEncodedString(new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", grantType),
        });

        string httpContent = await accessTokenRequest.HttpContent.ReadAsStringAsync();

        httpContent
            .Should()
            .Be(expectedHttpConent);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal async Task GivenGrantTypeIsAvailable_WhenAccessTokenRequestIsCreated_ThenHttpContentWithGrantTypeIsUsed(
        string grantType,
        string scope)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        var accessTokenRequest = new AccessTokenRequest(grantType, scope);

        // Then

        string expectedHttpConent = await CreateFormUrlEncodedString(new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("grant_type", grantType),
            new KeyValuePair<string, string>("scope", scope),
        });

        string httpContent = await accessTokenRequest.HttpContent.ReadAsStringAsync();

        httpContent
            .Should()
            .Be(expectedHttpConent);
    }

    private static async Task<string> CreateFormUrlEncodedString(List<KeyValuePair<string, string>> values)
    {
        using var formUrlEncodedContent = new FormUrlEncodedContent(values);
        return await formUrlEncodedContent.ReadAsStringAsync();
    }
}
