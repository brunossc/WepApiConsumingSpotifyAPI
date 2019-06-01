using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Domain.Model;
using Domain.Repository.Interfaces;
using Infrastructure.Repository.Base;

namespace Infrastructure.Repository
{
    public class CashbackRepository : DataRepository<CashBack>, ICashbackRepository
    {
        public CashbackRepository(MyDbContext context)
        : base(context)
        {
        }
    }

}