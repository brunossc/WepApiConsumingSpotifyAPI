using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Domain.Model;
using Domain.Repository.Interfaces;
using Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AlbumsRepository : DataRepository<Albums>, IAlbumsRepository
    {
        public AlbumsRepository(MyDbContext context)
        : base(context)
        {
        }

        public IQueryable<Albums> GetPage(int page, int perPage)
        {
            int skip = (page - 1) * perPage;
            return _objectSet.AsNoTracking().OrderBy(x=>x.Name).Skip(skip).Take(perPage);
        }

        public IQueryable<Albums> GetGenre(string genre)
        {
            return _objectSet.AsNoTracking().Where(c=>c.Genre == genre).OrderBy(x=>x.Name);
        }

        public IQueryable<Albums> GetGenrePage(string genre, int page, int perPage)
        {
            int skip = (page - 1) * perPage;
            return _objectSet.AsNoTracking().Where(c=>c.Genre == genre).OrderBy(x=>x.Name).Skip(skip).Take(perPage);
        }
    }
}