using AutoFixture.Xunit2;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Extensions;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Options;

public class OptionsBuilderFluentValidationExtensionsTests
{
    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenOptionsBuilderIsCreated_WhenAddClientCredentialOptionsValidationIsCalled_ThenExpectedIValidateOptionsImplementationIsRegistered(
        [Frozen] Mock<IServiceCollection> serviceCollectionMock,
        OptionsBuilder<SimpleOAuth2ClientOptions> optionsBuilder)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        _ = optionsBuilder.AddSimpleOAuth2ClientOptionsValidation();

        // Then
        serviceCollectionMock.Verify(_ => _.Add(It.Is<ServiceDescriptor>(sd => IsClientCredentialOptionsValidationRegistered(sd))), Times.Once);
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenOptionsBuilderIsCreated_WhenAddClientCredentialOptionsValidationIsCalled_ThenExpectedIValidatorImplementationIsRegistered(
        [Frozen] Mock<IServiceCollection> serviceCollectionMock,
        OptionsBuilder<SimpleOAuth2ClientOptions> optionsBuilder)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        _ = optionsBuilder.AddSimpleOAuth2ClientOptionsValidation();

        // Then
        serviceCollectionMock.Verify(_ => _.Add(It.Is<ServiceDescriptor>(sd => IsClientCredentialOptionsValidatorRegistered(sd))), Times.Once);
    }

    private static bool IsClientCredentialOptionsValidationRegistered(ServiceDescriptor serviceDescriptor) =>
        serviceDescriptor.Lifetime == ServiceLifetime.Singleton &&
        serviceDescriptor.ServiceType == typeof(IValidateOptions<SimpleOAuth2ClientOptions>) &&
        serviceDescriptor.ImplementationType == typeof(SimpleOAuth2ClientOptionsValidation);

    private static bool IsClientCredentialOptionsValidatorRegistered(ServiceDescriptor serviceDescriptor) =>
        serviceDescriptor.Lifetime == ServiceLifetime.Singleton &&
        serviceDescriptor.ServiceType == typeof(IValidator<SimpleOAuth2ClientOptions>) &&
        serviceDescriptor.ImplementationType == typeof(SimpleOAuth2ClientOptionsValidator);
}
