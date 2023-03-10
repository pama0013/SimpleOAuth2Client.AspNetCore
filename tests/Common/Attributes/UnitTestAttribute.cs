using Xunit.Sdk;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;

/// <summary>
/// Allow to group Unit-Tests by traits.
/// </summary>
[TraitDiscoverer("SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes.UnitTestTraitDiscoverer", "SimpleOAuth2Client.AspNetCore.UnitTests")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
internal sealed class UnitTestAttribute : Attribute, ITraitAttribute
{
    // No implementation needed!
}
