using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserWriteOnlyRepository writeOnlyRepository;
    private readonly IUserReadOnlyRepository readOnlyRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;
    private readonly PasswordEncripter criptografiaDeSenha;

    public RegisterUserUseCase(IUserWriteOnlyRepository writeOnlyRepository, IUserReadOnlyRepository readOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper, PasswordEncripter criptografiaDeSenha)
    {
        this.writeOnlyRepository = writeOnlyRepository;
        this.readOnlyRepository = readOnlyRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
        this.criptografiaDeSenha = criptografiaDeSenha;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = mapper.Map<Domain.Entities.User>(request);
        user.Password = criptografiaDeSenha.Encrypt(request.Password);

        await writeOnlyRepository.Add(user);
        await unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = request.Name,
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterUserValidator();
        var result = validator.Validate(request);

        var emailExist = await readOnlyRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

