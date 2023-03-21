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
    [Fact]
    internal void GivenIValidatorIsNull_WhenSimpleOAuth2ClientOptionsValidationIsCreated_ThenArgumentNullExceptionWithRelatedMessageIsThrown()
    {
        // Given
        IValidator<SimpleOAuth2ClientOptions> invalidValidator = null!;

        // When
        Action sutCreated = () => _ = new SimpleOAuth2ClientOptionsValidation(invalidValidator);

        // Then
        sutCreated
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("validator");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenIValidatorIsValid_WhenSimpleOAuth2ClientOptionsValidationIsCreated_ThenNoExceptionIsThrown(
        Mock<IValidator<SimpleOAuth2ClientOptions>> validatorMock)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        Action sutCreated = () => _ = new SimpleOAuth2ClientOptionsValidation(validatorMock.Object);

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
        SimpleOAuth2ClientOptions options,
        [Frozen] Mock<IValidator<SimpleOAuth2ClientOptions>> validatorMock,
        SimpleOAuth2ClientOptionsValidation sut)
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
    internal void GivenSimpleOAuth2ClientOptionsValidationIsCreated_WhenValidateIsCalledWithInvalidOptions_ThenArgumentNullExpcetionIsThrown(
        string name,
        SimpleOAuth2ClientOptionsValidation sut)
    {
        // Given
        SimpleOAuth2ClientOptions invalidOptions = null!;

        // When
        Action validateOptionsAction = () => _ = sut.Validate(name, invalidOptions);

        // Then
        validateOptionsAction
            .Should()
            .ThrowExactly<ArgumentNullException>()
            .WithParameterName("options");
    }

    [UnitTest]
    [Theory]
    [AutoMoqData]
    internal void GivenValidationErrorsAreAvailable_WhenValidateIsCalled_ThenValidateOptionsResultFailedIsReturned(
        string name,
        SimpleOAuth2ClientOptions options,
        Generator<ValidationFailure> validationFailuresGenerator,
        [Frozen] Mock<IValidator<SimpleOAuth2ClientOptions>> validatorMock,
        SimpleOAuth2ClientOptionsValidation sut)
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
