
using System.ComponentModel.DataAnnotations;

namespace RHCQS_Repositories.UnitOfWork;


public class UnitOfWork : IUnitOfWork, IDisposable
{
    //public RHCQSContext Context { get; }

    //public IAccountRepository ProductRepository => new AccountRepository(Context);

   

    //public UnitOfWork(JewelrySalesSystemContext context)
    //{
    //    Context = context;
    //}

    //public void Dispose()
    //{
    //    Context?.Dispose();
    //}

    //public int Commit()
    //{
    //    TrackChanges();
    //    return Context.SaveChanges();
    //}

    //public async Task<int> CommitAsync()
    //{
    //    TrackChanges();
    //    return await Context.SaveChangesAsync();
    //}

    //private void TrackChanges()
    //{
    //    var validationErrors = Context.ChangeTracker.Entries<IValidatableObject>()
    //        .SelectMany(e => e.Entity.Validate(null))
    //        .Where(e => e != ValidationResult.Success)
    //        .ToArray();
    //    if (validationErrors.Any())
    //    {
    //        var exceptionMessage = string.Join(Environment.NewLine,
    //            validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
    //        throw new Exception(exceptionMessage);
    //    }
    //}
}