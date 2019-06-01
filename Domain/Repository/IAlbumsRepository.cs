using System;
using System.Linq;
using Domain.Model;
using Domain.Repository.Base.Interfaces;

namespace Domain.Repository.Interfaces
{
    public interface IAlbumsRepository : IRepository<Albums>
    {
        IQueryable<Albums> GetPage(int page, int perPage);
        IQueryable<Albums> GetGenre(string genre);
        IQueryable<Albums> GetGenrePage(string genre, int page, int perPage);
    }
}