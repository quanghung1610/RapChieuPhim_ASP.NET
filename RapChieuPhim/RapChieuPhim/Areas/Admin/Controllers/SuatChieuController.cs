using PagedList;
using RapChieuPhim.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class SuatChieuController : Controller
    {
        // GET: Admin/SuatChieu
        Data db = new Data();
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var books = db.Suatchieux.OrderBy(g => g.IdSC);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.total = db.Suatchieux.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSC,TenSuat,phuthusc")] Suatchieu ghe)
        {
            if (ModelState.IsValid)
            {
                db.Suatchieux.Add(ghe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ghe);
        }
        public ActionResult Edit(int? id)
        {
            Suatchieu phim = db.Suatchieux.Find(id);
            return View(phim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSC,TenSuat,phuthusc")] Suatchieu ghe)
        {
            if (ModelState.IsValid)
            {
                db.Suatchieux.AddOrUpdate(ghe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ghe);
        }
        public ActionResult Delete(int? id)
        {
            Suatchieu phim = db.Suatchieux.Find(id);
            return View(phim);
        }

        // POST: Admin/Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suatchieu phim = db.Suatchieux.Find(id);
            db.Suatchieux.Remove(phim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}