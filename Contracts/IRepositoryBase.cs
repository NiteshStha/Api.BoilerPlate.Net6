using System.Linq.Expressions;

namespace Contract
{
    public interface IRepositoryBase<T>
    {
        /// <summary>
        /// Finds all rows of the entity
        /// </summary>
        /// <returns>List of all the rows of the entity</returns>
        Task<IEnumerable<T>> FindAll();

        /// <summary>
        /// Finds all rows of the entity matching the provided condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>List of all the rows of the entity matching the provided condition</returns>
        Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Find the single entity from the provided id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single entity value</returns>
        Task<T> FindById(int id);

        /// <summary>
        /// Find the single entity from the provided condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>Single entity value</returns>
        Task<T> FindById(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Creates a new entity or row in the table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Create(T entity);

        /// <summary>
        /// Updates the entity or row in the table
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Deletes the entity or row in the table
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
    }
}