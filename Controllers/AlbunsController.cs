using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using SpotifyApi;

namespace beblue.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbunsController : ControllerBase
    {
        private MyDbContext _context;
        public AlbunsController(MyDbContext context)
        {
            _context = context;


        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Albums>> Get()
        {
            string page = Request.Query["page"];
            string perpage = Request.Query["perpage"];
            string Genre = Request.Query["genre"];

            IEnumerable<Albums> list;
            int _page;
            int _perpage;

            if (Int32.TryParse(page, out _page) && Int32.TryParse(perpage, out _perpage))
            {
                int skip = (_page - 1) * _perpage;
                if (!String.IsNullOrEmpty(Genre))
                {
                    list = _context.Albums.Where(c=>c.Genre == Genre).OrderBy(x=>x.Name).Skip(skip).Take(_perpage);
                }
                else
                {
                     list = _context.Albums.OrderBy(x=>x.Name).Skip(skip).Take(_perpage);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Genre))
                {
                    list = _context.Albums.Where(c=>c.Genre == Genre).OrderBy(x=>x.Name);
                }
                else
                {
                    list = _context.Albums.OrderBy(x=>x.Name);
                }                
            }

            return list.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Albums> Get(int id)
        {
            return _context.Albums.Where(c=>c.AlbumsId == id).FirstOrDefault();
        }
    }
}
