using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpotifyApi;

namespace beblue.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private MyDbContext _context;
        public SalesController(MyDbContext context)
        {
            _context = context;


        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Sales>> Get()
        {
            string page = Request.Query["page"];
            string perpage = Request.Query["perpage"];
            string DateIni = Request.Query["dateini"];
            string DateFin = Request.Query["datefin"];

            IEnumerable<Sales> list;
            int _page;
            int _perpage;
            DateTime _dateIni;
            DateTime _dateFin;
            // I can get from a external configuration, but for a test only....
            CultureInfo culture = new CultureInfo("pt-br");

            if (Int32.TryParse(page, out _page) && Int32.TryParse(perpage, out _perpage))
            {
                int skip = (_page - 1) * _perpage;
                if (DateTime.TryParse(DateIni, culture, DateTimeStyles.None, out _dateIni)
                && DateTime.TryParse(DateFin, culture, DateTimeStyles.None, out _dateFin))
                {
                    list = _context.Sales.Include(x=>x.Albums).ThenInclude(a=>a.Album).Where(c => c.Date >= _dateIni && c.Date <= _dateFin).OrderBy(x => x.Date).Skip(skip).Take(_perpage);
                }
                else
                {
                    list = _context.Sales.Include(x=>x.Albums).ThenInclude(a=>a.Album).OrderBy(x => x.Date).Skip(skip).Take(_perpage);
                }
            }
            else
            {
                if (DateTime.TryParse(DateIni, culture, DateTimeStyles.None, out _dateIni)
                && DateTime.TryParse(DateFin, culture, DateTimeStyles.None, out _dateFin))
                {
                    list = _context.Sales.Include(x=>x.Albums).ThenInclude(a=>a.Album).Where(c => c.Date >= _dateIni && c.Date <= _dateFin).OrderBy(x => x.Date);
                }
                else
                {
                    list = _context.Sales.Include(x=>x.Albums).ThenInclude(a=>a.Album).OrderBy(x => x.Date);
                }
            }

            return list.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Sales> Get(int id)
        {
            Sales sale = _context.Sales.Include(x=>x.Albums).ThenInclude(a=>a.Album).Where(c => c.SalesId == id).FirstOrDefault();
            return sale;
        }

        // POST api/values
        [HttpPost]
        public void Post(int[] albumsIds)
        {
            int currentWeekday = (int)((DayOfWeek)Enum.Parse(typeof(DayOfWeek), DateTime.Now.DayOfWeek.ToString()));
            IList<Albums> albums = _context.Albums.Where(c => albumsIds.Contains(c.AlbumsId)).ToList();
            IList<CashBack> cashback = _context.CashBack.Where(c=>c.Weekday == currentWeekday).ToList();
            Albums album;
            Sales sale = new Sales();
            sale.Date = DateTime.Now;
            AlbumsSold albumSold;

            foreach (int id in albumsIds)
            {
                album = albums.Where(c => c.AlbumsId == id).FirstOrDefault();

                if (album != null)
                {
                    albumSold = new AlbumsSold();
                    albumSold.Album = album;
                    albumSold.Price = album.Price;
                    albumSold.CashBack = cashback.Where(c=>c.Genre == album.Genre).FirstOrDefault().value;
                    sale.Albums.Add(albumSold);
                }
            }

            _context.Sales.Add(sale);
            _context.SaveChanges();
        }
    }
}
