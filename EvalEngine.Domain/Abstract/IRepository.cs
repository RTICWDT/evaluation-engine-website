// -----------------------------------------------------------------------
// <copyright file="IRepository.cs" company="RTI, Inc.">
// Base repository interface
// See: http://www.remondo.net/repository-pattern-example-csharp/
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Abstract
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    /// <summary>
    /// Base repository interface
    /// </summary>
    /// <typeparam name="T">entity type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Inserts entity
        /// </summary>
        /// <param name="entity">entity to insert</param>
        void Insert(T entity);

        /// <summary>
        /// Deletes entity
        /// </summary>
        /// <param name="entity">entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Updates entity
        /// </summary>
        /// <param name="entity">entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Searches for entities of type T based on predicate.
        /// </summary>
        /// <param name="predicate">predicate filter</param>
        /// <returns>collection of type T entities</returns>
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets first or default entity based on predicate.
        /// </summary>
        /// <param name="predicate">predicate filter</param>
        /// <returns>entity of type T</returns>
        T SearchForFirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <returns>collection of type T entities</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets entity by id
        /// </summary>
        /// <param name="id">id of entity</param>
        /// <returns>entity of type T</returns>
        T GetById(int id);

        void DeleteAll(IEnumerable<T> listOfEntities);
    }
}