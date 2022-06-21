namespace Contract
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// The User Repository Instance
        /// </summary>
        IUserRepository User { get; }

        /// <summary>
        /// Commits all the changes asynchronously
        /// </summary>
        /// <returns></returns>
        Task Commit();

        /// <summary>
        /// Save all the changes synchronously
        /// </summary>
        void Save();
    }
}