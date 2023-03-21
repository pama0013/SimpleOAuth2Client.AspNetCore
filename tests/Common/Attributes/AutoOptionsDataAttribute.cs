using AutoFixture;
using AutoFixture.Xunit2;
using SimpleOAuth2Client.AspNetCore.Options;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;

/// <summary>
/// This attribute is used to provide valid data for option related Unit-Tests.
/// </summary>
internal sealed class AutoOptionsDataAttribute : AutoDataAttribute
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public AutoOptionsDataAttribute() : base(() => ConfigureFuxture())
    {
    }

    private static IFixture ConfigureFuxture()
    {
        IFixture fixture = new Fixture();

        fixture
            .Customize<SimpleOAuth2ClientOptions>(composer => composer
                .FromFactory((string clientId, string clientSecret, Uri tokenEndpoint) => CreateSimpleOAuth2ClientOptions(clientId, clientSecret, tokenEndpoint))
                .OmitAutoProperties());

        static SimpleOAuth2ClientOptions CreateSimpleOAuth2ClientOptions(string clientId, string clientSecret, Uri tokenEndpoint)
        {
            return new SimpleOAuth2ClientOptions
            {
                ClientCredentialOptions = new ClientCredentialOptions
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    TokenEndpoint = tokenEndpoint
                }
            };
        }

        return fixture;
    }
}
