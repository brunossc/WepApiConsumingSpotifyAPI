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
    public class SalesRepository : DataRepository<Sales>, ISalesRepository
    {
        public SalesRepository(MyDbContext context)
        : base(context)
        {
        }

        public IEnumerable<Sales> GetPage(int page, int perPage)
        {
            int skip = (page - 1) * perPage;
            return GetSalesWithInclude().OrderBy(x=>x.Date).Skip(skip).Take(perPage);
        }

        public IEnumerable<Sales> GetPerDate(DateTime dateini, DateTime datefin)
        {
            return GetSalesWithInclude().Where(c=>c.Date >= dateini && c.Date <= datefin)
            .OrderBy(x=>x.Date);
        }

        public IEnumerable<Sales> GetPerDatePage(DateTime dateini, DateTime datefin, int page, int perPage)
        {
            int skip = (page - 1) * perPage;
            return GetSalesWithInclude().Where(c=>c.Date >= dateini && c.Date <= datefin)
            .OrderBy(x=>x.Date).Skip(skip).Take(perPage);
        }
        
        private IQueryable<Sales> GetSalesWithInclude()
        {
            return _objectSet.Include(x=>x.Albums).ThenInclude(a=>a.Album);
        }
    }

}