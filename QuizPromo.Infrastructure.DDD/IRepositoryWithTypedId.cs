using System.Linq;

namespace QuizPromo.Infrastructure.DDD
{
    public interface IRepositoryWithTypedId { }
    public interface IRepositoryWithTypedId<T, TId> : IRepositoryWithTypedId
    {
        /// <summary>
        /// Delete the specified object from the repository
        /// </summary>
        /// <typeparam name="T">Type of entity to be deleted</typeparam>
        /// <param name="target">Entity to delete</param>
        void Delete(T target);

        /// <summary>
        /// Delete the object that matches the supplied id from the repository
        /// </summary>
        /// <typeparam name="TId">Type of Id of the entity to be deleted</typeparam>
        /// <param name="id">Id of the entity to delete</param>
        void Delete(TId id);

        /// <summary>
        /// Save the specified object to the repository
        /// </summary>
        /// <typeparam name="T">Type of entity to save</typeparam>
        /// <param name="entity">Entity to save</param>
        void Save(T entity);

        /// <summary>
        /// Update the specified object to the repository. Works better and predictable than Save method
        /// </summary>
        /// <typeparam name="T">Type of entity to save</typeparam>
        /// <param name="entity">Entity to save</param>
        void Edit(T entity);

        /// <summary>
        /// Finds an item by id.
        /// </summary>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <param name="id">The id of the entity</param>
        /// <returns>The matching item</returns>
        T GetById(TId id);

        /// <summary>
        /// Finds an item by a specification
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <returns>The the matching item</returns>
        T GetBySpecification(ISimpleSpecification<T> specification);

        /// <summary>
        /// Finds all items within the repository.
        /// </summary>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <returns>All items in the repository</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Finds all items by a specification.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <typeparam name="T">Type of entity to find</typeparam>
        /// <returns>All matching items</returns>
        IQueryable<T> GetAllBySpecification(ISimpleSpecification<T> specification);
    }
}