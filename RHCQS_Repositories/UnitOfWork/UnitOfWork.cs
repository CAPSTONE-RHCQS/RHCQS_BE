using RHCQS_DataAccessObjects;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Repositories.Repo.Implement;
using RHCQS_Repositories.Repo.Interface;
using System.ComponentModel.DataAnnotations;

namespace RHCQS_Repositories.UnitOfWork;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    public RhcqsContext Context { get; }
    private Dictionary<Type, object> _repositories;

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
    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        _repositories ??= new Dictionary<Type, object>();
        if (_repositories.TryGetValue(typeof(TEntity), out object repository))
        {
            return (IGenericRepository<TEntity>)repository;
        }

        repository = new GenericRepository<TEntity>(Context);
        _repositories.Add(typeof(TEntity), repository);
        return (IGenericRepository<TEntity>)repository;
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