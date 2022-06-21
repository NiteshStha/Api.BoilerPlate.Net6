using Contract;
using Entities;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RepositoryContext _repositoryContext;
        private IUserRepository _user;

        public IUserRepository User => _user ??= new UserRepository(_repositoryContext);

        public UnitOfWork(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task Commit() => await _repositoryContext.SaveChangesAsync();

        public void Save() => _repositoryContext.SaveChanges();
    }
}