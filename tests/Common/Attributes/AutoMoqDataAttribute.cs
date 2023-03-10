using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;

/// <summary>
/// This attribute is used to provide Mocks automatically to XUnit based Unit-Tests.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal sealed class AutoMoqDataAttribute : AutoDataAttribute
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}
