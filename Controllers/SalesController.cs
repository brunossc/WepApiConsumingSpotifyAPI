using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class SalesController : ControllerBase
    {
        private ISalesRepository _repo;
        private IAlbumsRepository _repoAlbums;
        private ICashbackRepository _repoCash;

        public SalesController(ISalesRepository repo, IAlbumsRepository repoAlbums,
         ICashbackRepository repoCash)
        {
            _repo = repo;
            _repoAlbums = repoAlbums;
            _repoCash = repoCash;
        }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Sales>> Get()
        {
            string page = Request.Query["page"];
            string perpage = Request.Query["perpage"];
            string DateIni = Request.Query["dateini"];
            string DateFin = Request.Query["datefin"];

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
                    return _repo.GetPerDatePage(_dateIni, _dateFin, _page, _perpage).ToList();
                }
                else
                {
                    return _repo.GetPage(_page, _perpage).ToList();
                }
            }
            else
            {
                if (DateTime.TryParse(DateIni, culture, DateTimeStyles.None, out _dateIni)
                && DateTime.TryParse(DateFin, culture, DateTimeStyles.None, out _dateFin))
                {
                    return _repo.GetPerDate(_dateIni, _dateFin).ToList();
                }
                else
                {
                    return _repo.GetAll().OrderBy(c=>c.Date).ToList();
                }
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<Sales> Get(int id)
        {
            Sales sale = _repo.Find(c=>c.SalesId == id).FirstOrDefault();
            return sale;
        }

        // POST api/values
        [HttpPost]
        public void Post(int[] albumsIds)
        {
            int currentWeekday = (int)((DayOfWeek)Enum.Parse(typeof(DayOfWeek), DateTime.Now.DayOfWeek.ToString()));
            IList<Albums> albums = _repoAlbums.Find(c => albumsIds.Contains(c.AlbumsId)).ToList();
            IList<CashBack> cashback = _repoCash.Find(c=>c.Weekday == currentWeekday).ToList();
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
                    albumSold.AlbumsId = album.AlbumsId;
                    albumSold.Price = album.Price;
                    albumSold.CashBack = cashback.Where(c=>c.Genre == album.Genre).FirstOrDefault().value;
                    sale.Albums.Add(albumSold);
                }
            }

            _repo.Add(sale);
            _repo.SaveChanges();
        }
    }
}
