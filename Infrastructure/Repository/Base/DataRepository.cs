using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Database;
using Domain.Repository.Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Base
{
    /// <summary>
    /// A generic repository for working with data in the database
    /// </summary>
    /// <typeparam name="T">A POCO that represents an Entity Framework entity</typeparam>
    public abstract class DataRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// The context object for the database
        /// </summary>
        private DbContext _context;

        public DbContext currentDbContext
        {
            get { return _context; }
        }

        /// <summary>
        /// The IObjectSet that represents the current entity.
        /// </summary>
        protected DbSet<T> _objectSet;

        /// <summary>
        /// Initializes a new instance of the DataRepository class
        /// </summary>
        /// <param name="context">The Entity Framework ObjectContext</param>
        public DataRepository(MyDbContext context)
        {
            _context = context;
            //_context.Configuration.EnsureTransactionsForFunctionsAndCommands = true;
            _objectSet = _context.Set<T>();
        }

        /// <summary>
        /// Gets all records as an IQueryable
        /// </summary>
        /// <returns>An IQueryable object containing the results of the query</returns>
        public virtual IQueryable<T> Fetch()
        {
            return _objectSet;
        }

        /// <summary>
        /// Gets all records as an IEnumberable
        /// </summary>
        /// <returns>An IEnumberable object containing the results of the query</returns>
        public IEnumerable<T> GetAll()
        {
            return Fetch().AsEnumerable();
        }

        /// <summary>
        /// Gets all records as an IEnumberable for page
        /// </summary>
        /// <returns>An IEnumberable object containing the results of the query</returns>
        public virtual IEnumerable<T> GetPage(ref int totalRows, int page, int count, Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds a record with the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A collection containing the results of the query</returns>
        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _objectSet.Where(predicate);
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public T Single(Func<T, bool> predicate)
        {
            return _objectSet.Single(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public T First(Func<T, bool> predicate)
        {
            return _objectSet.First(predicate);
        }

        /// <summary>
        /// verify if exist record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>True if exists</returns>
        public bool Exists(Func<T, bool> predicate)
        {
            return _objectSet.Where(predicate).Count() > 0;
        }

        /// <summary>
        /// Deletes the specified entitiy
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public virtual void Delete(T entity)
        {
            //throw new DataMisalignedException();
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.Remove(entity);
        }

        /// <summary>
        /// Deletes records matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        public void Delete(Func<T, bool> predicate)
        {
            //throw new DataMisalignedException();
            IEnumerable<T> records = from x in _objectSet.Where(predicate) select x;

            foreach (T record in records)
            {
                _objectSet.Remove(record);
            }
        }

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="entity"/> is null</exception>
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            _objectSet.Add(entity);
        }

        /// <summary>
        /// Attaches the specified entity
        /// </summary>
        /// <param name="entity">Entity to attach</param>
        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void DeleteAll()
        {
            _objectSet.RemoveRange(_objectSet.ToList());
            _context.SaveChanges();
        }

        /// <summary>
        /// Saves all context changes
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex is DbUpdateException)
                {
                    if (ex.GetBaseException() is System.Data.SqlClient.SqlException && ((System.Data.SqlClient.SqlException)ex.GetBaseException()).Number == 2601)
                    {
                        throw new Exception("InsertDuplicateException",ex.GetBaseException());
                    }
                    else if (ex.GetBaseException() is System.Data.SqlClient.SqlException && ((System.Data.SqlClient.SqlException)ex.GetBaseException()).Number == 547)
                    {
                        throw new Exception("HasReferenceException",ex.GetBaseException());
                    }
                    else
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves all context changes with the specified SaveOptions
        /// </summary>
        /// <param name="options">Options for saving the context</param>
        private void SaveChanges(SaveOptions options)
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

    }
}
