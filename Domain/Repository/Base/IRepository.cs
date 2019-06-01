using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repository.Base.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        DbContext currentDbContext
        {
            get;
        }
        IQueryable<T> Fetch();
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Func<T, bool> predicate);
        T Single(Func<T, bool> predicate);
        T First(Func<T, bool> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Delete(Func<T, bool> predicate);
        void DeleteAll();
        void Attach(T entity);
        void Detach(T entity);
        void SaveChanges();
        bool Exists(Func<T, bool> predicate);
    }
}