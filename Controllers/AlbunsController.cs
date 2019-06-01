using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Domain.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApiSpotify.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {

        private IAlbumsRepository _repo;
        public AlbumsController(IAlbumsRepository repo)
        {
            _repo = repo;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Albums>> Get()
        {
            string page = Request.Query["page"];
            string perPage = Request.Query["perpage"];
            string genre = Request.Query["genre"];

            int _page;
            int _perPage;

            IQueryable<Albums> albumQuery = _repo.Fetch();

            if (Int32.TryParse(page, out _page) && Int32.TryParse(perPage, out _perPage))
            {
                if (!String.IsNullOrEmpty(genre))
                    return _repo.GetGenrePage(genre, _page, _perPage).ToList();
                else
                    return _repo.GetPage(_page, _perPage).ToList();
            }
            else
            {
                if (!String.IsNullOrEmpty(genre))
                    return _repo.GetGenre(genre).ToList();
                else
                    return _repo.GetAll().OrderBy(x => x.Name).ToList();
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Albums> Get(int id)
        {
            return _repo.Find(c => c.AlbumsId == id).FirstOrDefault();
        }
    }
}
