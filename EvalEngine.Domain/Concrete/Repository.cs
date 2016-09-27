// -----------------------------------------------------------------------
// <copyright file="Repository.cs" company="MPR INC">
//      Copyright (c) MPR Inc. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace EvalEngine.Domain.Concrete
{
    using System;
    using System.Data.Linq;
    using System.Linq;
    using System.Linq.Expressions;
    using EvalEngine.Domain.Abstract;
    using System.Collections.Generic;

    /// <summary>
    /// An implementation of a repository for a generic class.
    /// </summary>
    /// <typeparam name="T">A class type.</typeparam>
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// The table in the database.
        /// </summary>
        private Table<T> dataTable;

        /// <summary>
        /// Initializes a new instance of the Repository class.
        /// </summary>
        /// <param name="connectionString">a connection string to the database.</param>
        public Repository(string connectionString)
        {
            this.dataTable = (new DataContext(connectionString)).GetTable<T>();
        }

        /// <summary>
        /// Inserts an generic entity into the database.
        /// </summary>
        /// <param name="entity">An entity to insert.</param>
        public void Insert(T entity)
        {
            this.dataTable.InsertOnSubmit(entity);
            this.dataTable.Context.SubmitChanges();
        }

        /// <summary>
        /// Deletes a generic entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public void Delete(T entity)
        {
            this.dataTable.DeleteOnSubmit(entity);
            this.dataTable.Context.SubmitChanges();
        }

        /// <summary>
        /// Updates or inserts a generic entity into the database.
        /// </summary>
        /// <param name="entity">The entity to update or insert.</param>
        public void Update(T entity)
        {
            if (entity.Id.Equals(0))
            {
                this.Insert(entity);
            }
            else
            {
                if (this.dataTable.GetOriginalEntityState(entity) == null)
                {
                    this.dataTable.Attach(entity);
                }

                this.dataTable.Context.Refresh(RefreshMode.KeepCurrentValues, entity);
                this.dataTable.Context.SubmitChanges();
            }
        }

        /// <summary>
        /// Returns a list (IQueryable) of entities that return true to the predicate.
        /// </summary>
        /// <param name="predicate">The expression you want to match on.</param>
        /// <returns>An IQueryable containing the entities filtered using the predicate.</returns>
        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return this.dataTable.Where(predicate);
        }

        /// <summary>
        /// Gets a list of the records stored in the database.
        /// </summary>
        /// <returns>An IQueryable with all the items in the table.</returns>
        public IQueryable<T> GetAll()
        {
            return this.dataTable;
        }

        /// <summary>
        /// Returns an entity that has the id passed as argument.
        /// </summary>
        /// <param name="id">The id of the entity you want to retrieve.</param>
        /// <returns>Null if there is no entity with the specified id. The entity otherwise.</returns>
        public T GetById(int id)
        {
            return this.dataTable.FirstOrDefault(m => m.Id.Equals(id));
        }

        /// <summary>
        /// Returns a single entity that matches the predicate.
        /// </summary>
        /// <param name="predicate">An expression to filter on.</param>
        /// <returns>Null if there is no entity matching the criteria.  The entity otherwise.</returns>
        public T SearchForFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.dataTable.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Deletes a list of entities from the database.
        /// </summary>
        /// <param name="listOfEntities">The collections of entities to delete.</param>
        public void DeleteAll(IEnumerable<T> listOfEntities)
        {
            this.dataTable.DeleteAllOnSubmit(listOfEntities);
            this.dataTable.Context.SubmitChanges();
        }
    }
}