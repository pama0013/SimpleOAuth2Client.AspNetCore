// ---------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTestTraitDiscoverer.cs" company="Vereinigte Lohnsteuerhilfe e.V">
// copyright (c) 2022 Vereinigte Lohnsteuerhilfe e.V
//               Fritz-Voigt-Str. 13
//               67433 Neustadt/Wstr.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes
{
    /// <summary>
    /// This is a TraitDiscoverer implementation. Discover all Tests marked by the "UnitTest" attribute.
    /// </summary>
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "This class is used by Xunit Testframwork")]
    internal sealed class UnitTestTraitDiscoverer : ITraitDiscoverer
    {
        private const string KEY = "Category";
        private const string VALUE = "UnitTest";

        /// <inheritdoc/>
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            yield return new KeyValuePair<string, string>(KEY, VALUE);
        }
    }
}
