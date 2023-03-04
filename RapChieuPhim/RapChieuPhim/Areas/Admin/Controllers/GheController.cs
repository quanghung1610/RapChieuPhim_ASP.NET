using PagedList;
using RapChieuPhim.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class GheController : Controller
    {
        // GET: Admin/Ghe
        Data db = new Data();
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var books = db.Ghes.OrderBy(g => g.Id_phong);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewData["loai"] = db.LoaiGhes.AsQueryable().ToList();
            ViewData["phong"] = db.Phongs.AsQueryable().ToList();
            ViewBag.total = db.Ghes.ToList().Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Phims/Create
        public ActionResult Create()
        {
            ViewBag.Loai_id = new SelectList(db.LoaiGhes, "Id", "TenLoaiGhe");
            ViewData["ghe"] = db.ghe_phong.AsQueryable().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ghe_id,Loai_id,TringTrang,Id_phong,TenGhe,HangGhe")] Ghe ghe)
        {
            if (ghe.Id_phong != null)
            {
                var id = Convert.ToInt32(Request["Id_phong"]);
                var somoihang = db.Phongs.FirstOrDefault(g => g.Id == id).Soluongghemoihang;
                var hang = Request["hang"];
                var dem = db.Ghes.Where(g => g.HangGhe == hang && g.Id_phong == ghe.Id_phong).Count();
                if (somoihang > dem)
                {
                    if (ModelState.IsValid)
                    {
                        ghe.Id_phong = id;
                        ghe.HangGhe = hang;
                        ghe.TringTrang = false;
                        db.Ghes.Add(ghe);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Ghe", new { sc = 1 });
                }
            }

            ViewBag.Loai_id = new SelectList(db.LoaiGhes, "Id", "TenLoaiGhe");
            ViewData["ghe"] = db.ghe_phong.AsQueryable().ToList();
            return View(ghe);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ghe phim = db.Ghes.Find(id);
            ViewData["loai"] = db.LoaiGhes.ToList();
            ViewData["phong"] = db.Phongs.ToList();
            return View(phim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ghe_id,Loai_id,TringTrang,Id_phong,TenGhe,HangGhe")] Ghe ghe)
        {
            if (ModelState.IsValid)
            {
                ghe.Id_phong = Convert.ToInt32(Request["Id_phong"]);
                ghe.Loai_id = Convert.ToInt32(Request["Loai_id"]);
                db.Ghes.AddOrUpdate(ghe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["loai"] = db.LoaiGhes.ToList();
            ViewData["phong"] = db.Phongs.ToList();
            return View(ghe);
        }
        public ActionResult Delete(int? id)
        {
            ViewData["loai"] = db.LoaiGhes.ToList();
            ViewData["phong"] = db.Phongs.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ghe phim = db.Ghes.Find(id);
            return View(phim);
        }

        // POST: Admin/Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewData["loai"] = db.LoaiGhes.ToList();
            ViewData["phong"] = db.Phongs.ToList();
            Ghe phim = db.Ghes.Find(id);
            db.Ghes.Remove(phim);
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