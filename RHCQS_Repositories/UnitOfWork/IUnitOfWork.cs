

using RHCQS_Repositories.Repo.Interface;

namespace RHCQS_Repositories.UnitOfWork;

	public interface IUnitOfWork : IDisposable, IGenericRepositoryFactory
{

	int Commit();

	Task<int> CommitAsync();
}
