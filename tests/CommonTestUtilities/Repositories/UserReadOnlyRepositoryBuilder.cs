using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> repository;

    public UserReadOnlyRepositoryBuilder() => repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email)
    {
        repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public IUserReadOnlyRepository Build() => repository.Object;
}

