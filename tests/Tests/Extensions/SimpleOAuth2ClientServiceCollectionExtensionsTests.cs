using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SimpleOAuth2Client.AspNetCore.Contracts;
using SimpleOAuth2Client.AspNetCore.Extensions;
using SimpleOAuth2Client.AspNetCore.GrantTypes.Contracts;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Extensions;

public class SimpleOAuth2ClientServiceCollectionExtensionsTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenConfigureOptionsActionAndIServiceCollectionAreAvailable_WhenAddSimpleOAuth2ClientIsCalled_ThenAllRelatedServicesAreRegistered(
        ServiceCollection services,
        Uri tokenEndpoint,
        string clientId,
        string clientSecret)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        _ = services.AddSimpleOAuth2Client(options =>
        {
            options.ClientCredentialOptions.ClientId = clientId;
            options.ClientCredentialOptions.ClientSecret = clientSecret;
            options.ClientCredentialOptions.TokenEndpoint = tokenEndpoint;
        });

        // Then
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        AssertRegisteredService<IAuthorizationGrant>(serviceProvider);
        AssertRegisteredService<IOAuth2Client>(serviceProvider);
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenConfigureOptionsActionIsNull_WhenAddSimpleOAuth2ClientIsCalled_ThenArgumentNullExceptionWithRelatedMessageIsThrown(
        ServiceCollection services)
    {
        // Given
        Action<SimpleOAuth2ClientOptions> invalidConfigureOptions = null!;

        // When
        Action addSimpleOAuth2ClientCalled = () => _ = services.AddSimpleOAuth2Client(invalidConfigureOptions);

        // Then
        addSimpleOAuth2ClientCalled
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("configureOptions");
    }

    [UnitTest]
    [Fact]
    internal void GivenIServiceCollectionIsNull_WhenAddSimpleOAuth2ClientIsCalled_ThenArgumentNullExceptionWithRelatedMessageIsThrown()
    {
        // Given
        IServiceCollection services = null!;

        // When
        Action addSimpleOAuth2ClientCalled = () => _ = services.AddSimpleOAuth2Client(options => { });

        // Then
        addSimpleOAuth2ClientCalled
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("services");
    }

    private static void AssertRegisteredService<TService>(IServiceProvider serviceProvider) where TService : notnull
    {
        Action isServiceRegistered = () => _ = serviceProvider.GetRequiredService<TService>();

        isServiceRegistered
            .Should()
            .NotThrow<InvalidOperationException>();
    }
}
