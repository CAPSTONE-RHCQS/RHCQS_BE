

using RHCQS_Repositories.Repo.Interface;

namespace RHCQS_Repositories.UnitOfWork;

	public interface IUnitOfWork : IDisposable
{
	public IAccountRepository AccountRepository { get; }
	public IRoleRepository RoleRepository { get; }

	public IProjectRepository ProjectRepository { get; }


	int Commit();

	Task<int> CommitAsync();
}
