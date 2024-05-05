using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }

    [Fact]
    public void Error_Email_Empty()
    {
        // Arrange
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        // Arrange
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email.com";

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        // Arrange
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_GREATER_THAN_OR_EQUAL_TO));
    }


}

