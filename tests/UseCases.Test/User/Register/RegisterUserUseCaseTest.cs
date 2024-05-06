using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async void Success()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);

    }

    [Fact]
    public async void Error_Email_Already_Registered()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);
                
        Func<Task> act = async () => await useCase.Execute(request);

        //Act and Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

    }

    [Fact]
    public async void Error_Name_Empty()
    {
        // Arrange
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();
                
        Func<Task> act = async () => await useCase.Execute(request);

        //Act and Assert
        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(e => e.ErrorMessages.Count == 1 && e.ErrorMessages.Contains(ResourceMessagesException.NAME_EMPTY));
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var readRepositoryBuilder  = new UserReadOnlyRepositoryBuilder();
        var writeRepository = UserWriteOnlyRepositoryBuilder.Build();

        if (!string.IsNullOrEmpty(email))
        {
            readRepositoryBuilder.ExistActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(writeRepository, readRepositoryBuilder.Build(), unitOfWork, mapper, passwordEncripter);
    }
}

