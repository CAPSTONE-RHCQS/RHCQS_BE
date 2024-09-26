
using RHCQS_DataAccessObjects;
using RHCQS_Repositories.Repo.Implement;
using RHCQS_Repositories.Repo.Interface;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_Repositories.UnitOfWork;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    public RhcqsContext Context { get; }

    public IAccountRepository AccountRepository => new AccountRepository(Context);
    public IRoleRepository RoleRepository => new RoleRepository(Context);

    public IProjectRepository ProjectRepository => new ProjectRepository(Context);


    public UnitOfWork(RhcqsContext context)
    {
        Context = context;
    }

    public void Dispose()
    {
        Context?.Dispose();
    }

    public int Commit()
    {
        TrackChanges();
        return Context.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        TrackChanges();
        return await Context.SaveChangesAsync();
    }

    private void TrackChanges()
    {
        var validationErrors = Context.ChangeTracker.Entries<IValidatableObject>()
            .SelectMany(e => e.Entity.Validate(null))
            .Where(e => e != ValidationResult.Success)
            .ToArray();
        if (validationErrors.Any())
        {
            var exceptionMessage = string.Join(Environment.NewLine,
                validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
            throw new Exception(exceptionMessage);
        }
    }
}