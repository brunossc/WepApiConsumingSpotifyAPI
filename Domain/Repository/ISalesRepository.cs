using System;
using System.Collections.Generic;
using Domain.Model;
using Domain.Repository.Base.Interfaces;

namespace Domain.Repository.Interfaces
{
    public interface ISalesRepository : IRepository<Sales>
    {
        IEnumerable<Sales> GetPage(int page, int perPage);
        IEnumerable<Sales> GetPerDate(DateTime dateini, DateTime datefin);
        IEnumerable<Sales> GetPerDatePage(DateTime dateini, DateTime datefin, int page, int perPage);
    }
}