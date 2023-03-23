using AutoFixture.Xunit2;
using FluentValidation.TestHelper;
using SimpleOAuth2Client.AspNetCore.Options;
using SimpleOAuth2Client.AspNetCore.Options.Validation.Validators;
using SimpleOAuth2Client.AspNetCore.UnitTests.Common.Attributes;
using Xunit;

namespace SimpleOAuth2Client.AspNetCore.UnitTests.Tests.Options;

public class RetryOptionsValidatorTests
{
    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenFirstRetryDelayIsNegative_WhenValidatorIsExecuted_ThenValidationErrorForFirstRetryDelayIsAvailable(
        RetryOptions options,
        RetryOptionsValidator sut)
    {
        // Given
        options.FirstRetryDelay = -1;

        // When
        TestValidationResult<RetryOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.FirstRetryDelay)
            .WithErrorMessage("The FirstRetryDelay can not be negative. Please add a value that is greater or equal then zero.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenRetryAttemptsIsNegative_WhenValidatorIsExecuted_ThenValidationErrorForRetryAttemptsIsAvailable(
        RetryOptions options,
        RetryOptionsValidator sut)
    {
        // Given
        options.RetryAttempts = -1;

        // When
        TestValidationResult<RetryOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.RetryAttempts)
            .WithErrorMessage("The RetryAttempts can not be negative. Please add a value that is greater or equal then zero.");
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenRetryOptionsAreValid_WhenValidatorIsExecuted_ThenNoValidationErrorsAreAvailable(
        RetryOptions options,
        RetryOptionsValidator sut)
    {
        // Given
        // Nothing to do --> Test data will be injected (See: AutoData attribute)

        // When
        TestValidationResult<RetryOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults.ShouldNotHaveAnyValidationErrors();
    }

    [UnitTest]
    [Theory]
    [AutoData]
    internal void GivenTimeoutPerRetryIsNegative_WhenValidatorIsExecuted_ThenValidationErrorFoTimeoutPerRetryIsAvailable(
        RetryOptions options,
        RetryOptionsValidator sut)
    {
        // Given
        options.TimeoutPerRetry = -1;

        // When
        TestValidationResult<RetryOptions> validationResults = sut.TestValidate(options);

        // Then
        validationResults
            .ShouldHaveValidationErrorFor(_ => _.TimeoutPerRetry)
            .WithErrorMessage("The TimeoutPerRetry can not be negative. Please add a value that is greater or equal then zero.");
    }
}
