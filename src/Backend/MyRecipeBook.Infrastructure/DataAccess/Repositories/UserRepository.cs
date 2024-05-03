using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly MyRecipeBookDbContext dbContext;

    public UserRepository(MyRecipeBookDbContext dbContext) => this.dbContext = dbContext;

    public async Task Add(User user) => await dbContext.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
}