using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyRecipeBookDbContext dbContext;

    public UnitOfWork(MyRecipeBookDbContext dbContext) => this.dbContext = dbContext;

    public async Task Commit() => await dbContext.SaveChangesAsync();
}

