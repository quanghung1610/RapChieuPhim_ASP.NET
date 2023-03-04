using PagedList;
using RapChieuPhim.Models;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class VeController : Controller
    {
        // GET: Admin/Ve
        Data db = new Data();
        public ActionResult Index(int? page,string Ngay="",string Ngay1="")
        {
            if (page == null) page = 1;
            var books = db.Ve_LC_Phim.AsQueryable().OrderBy(g => g.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.total = db.Ghes.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
    }
}