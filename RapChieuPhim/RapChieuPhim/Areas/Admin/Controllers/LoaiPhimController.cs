using PagedList;
using RapChieuPhim.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class LoaiPhimController : Controller
    {
        private Data db = new Data();

        // GET: Admin/Phongs
        public ActionResult Index(int? page, string keysearch = "")
        {
            if (page == null) page = 1;
            var books = db.LoaiPhims.OrderBy(g => g.Id);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.total = db.LoaiPhims.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Phongs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phong phong = db.Phongs.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // GET: Admin/Phongs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Phongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenLoai")] LoaiPhim phong)
        {
            if (ModelState.IsValid)
            {
                db.LoaiPhims.Add(phong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phong);
        }

        // GET: Admin/Phongs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhim phong = db.LoaiPhims.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Admin/Phongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenLoai")] LoaiPhim phong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phong);
        }

        // GET: Admin/Phongs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiPhim phong = db.LoaiPhims.Find(id);
            if (phong == null)
            {
                return HttpNotFound();
            }
            return View(phong);
        }

        // POST: Admin/Phongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiPhim phong = db.LoaiPhims.Find(id);
            db.LoaiPhims.Remove(phong);
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