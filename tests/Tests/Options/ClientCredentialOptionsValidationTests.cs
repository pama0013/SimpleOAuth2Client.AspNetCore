using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Options;

public class ClientCredentialOptionsValidationTests
{
    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenClientCredentialOptionsValidationIsCreated_WhenValidateIsCalledWithInvalidOptions_ThenArgumentNullExpcetionIsThrown(
        string name,
        ClientCredentialOptionsValidation sut)
    {
        // Given
        ClientCredentialOptions invalidOptions = null!;

        // When
        Action validateOptionsAction = () => _ = sut.Validate(name, invalidOptions);

        // Then
        validateOptionsAction
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("options");
    }

    [UnitTest]
    [Fact]
    internal void GivenIValidatorIsNull_WhenClientCredentialOptionsValidationIsCreated_ThenArgumentNullExceptionWithRelatedMessageIsThrown()
    {
        // Given
        IValidator<ClientCredentialOptions> invalidValidator = null!;

        // When
        Action sutCreated = () => _ = new ClientCredentialOptionsValidation(invalidValidator);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("validator");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIValidatorIsValid_WhenClientCredentialOptionsValidationIsCreated_ThenNoExceptionIsThrown(
        Mock<IValidator<ClientCredentialOptions>> validatorMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new ClientCredentialOptionsValidation(validatorMock.Object);

        // Then
        sutCreated
            .Should()
            .NotThrow<ArgumentNullException>();
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenNoValidationErrorsAreAvailable_WhenValidateIsCalled_ThenValidateOptionsResultSucceededIsReturned(
        string name,
        ClientCredentialOptions options,
        [Frozen] Mock<IValidator<ClientCredentialOptions>> validatorMock,
        ClientCredentialOptionsValidation sut)
    {
        // Given
        validatorMock
            .Setup(_ => _.Validate(options))
            .Returns(new ValidationResult());

        // When
        ValidateOptionsResult validateOptionsResult = sut.Validate(name, options);

        // Then
        validateOptionsResult
            .Succeeded
            .Should()
            .BeTrue();
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenValidationErrorsAreAvailable_WhenValidateIsCalled_ThenValidateOptionsResultFailedIsReturned(
        string name,
        ClientCredentialOptions options,
        Generator<ValidationFailure> validationFailuresGenerator,
        [Frozen] Mock<IValidator<ClientCredentialOptions>> validatorMock,
        ClientCredentialOptionsValidation sut)
    {
        // Given
        validatorMock
            .Setup(_ => _.Validate(options))
            .Returns(new ValidationResult(validationFailuresGenerator.Take(1)));

        // When
        ValidateOptionsResult validateOptionsResult = sut.Validate(name, options);

        // Then
        validateOptionsResult
            .Failed
            .Should()
            .BeTrue();
    }
}
