using PagedList;
using RapChieuPhim.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class LichChieuController : Controller
    {
        Data db = new Data();
        public ActionResult Index(int? page,string keysearch="")
        {
            if (page == null) page = 1;
            var books = db.LichChieux.OrderBy(g => g.IdSC);
            if (!string.IsNullOrEmpty(keysearch))
            {
                var IdPhim = db.Phims.FirstOrDefault(g => g.TenPhim.Contains(keysearch)).Id;
                books = books.Where(g => g.IdPhim == IdPhim).OrderBy(g => g.Id);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewData["phong"] = db.Phongs.AsQueryable().ToList();
            ViewData["phim"] = db.Phims.AsQueryable().ToList();
            ViewBag.total = db.LichChieux.ToList().Count();
            ViewData["ca"] = db.CaChieux.AsQueryable().ToList();
            ViewData["suat"] = db.Suatchieux.AsQueryable().ToList();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            ViewData["phong"] = db.Phongs.AsQueryable().Where(g => g.TrinhTrang == true).ToList();
            ViewData["phim"] = db.Phims.AsQueryable().Where(g => g.TinhTrang == true).ToList();
            ViewBag.total = db.LichChieux.ToList().Count();
            ViewData["ca"] = db.CaChieux.AsQueryable().ToList();
            ViewData["suat"] = db.Suatchieux.AsQueryable().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NgayChieu,Thu,Phuthulc,IdPhim,IdCC,IdSC,GioBD,GioKT,IdPhong,TenSuat,phuthusc")] LichChieu ghe)
        {
            if (ModelState.IsValid)
            {
                var m = db.LichChieux.Where(g => g.NgayChieu == ghe.NgayChieu && g.IdPhong == ghe.IdPhong).ToList();
                if (m != null)
                {
                    foreach (var item in m)
                        if (item.GioBD == ghe.GioBD || item.GioKT >= ghe.GioBD)
                        {
                            return RedirectToAction("Create", "LichChieu", new { sc = 2 });
                        }
                }
                if (ghe.NgayChieu < DateTime.Now)
                {
                    return RedirectToAction("Create", "LichChieu", new { sc = 1 });
                }
                else
                {
                    var thu = ((int)ghe.NgayChieu.DayOfWeek);
                    if (thu == 6 || thu == 0)
                    {
                        ghe.Phuthulc = 30000;
                    }
                    else
                    {
                        ghe.Phuthulc = 0;
                    }
                    if (thu == 0)
                    {
                        ghe.Thu = "Chủ nhật";
                    }
                    else
                    {
                        thu = thu + 1;
                        ghe.Thu = "Thứ " + thu.ToString();
                    }

                    ghe.IdPhong = Convert.ToInt32(Request["IdPhong"]);
                    var phut = db.Phims.FirstOrDefault(p => p.Id == ghe.IdPhim).ThoiLuong;
                    var gio = phut % 60;
                    var phut1 = phut - gio * 60;
                    TimeSpan duration = new TimeSpan(gio, phut1, 00);
                    ghe.GioKT = ghe.GioBD + duration;
                    db.LichChieux.Add(ghe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(ghe);
        }
        public ActionResult Edit(int? id)
        {
            ViewData["phong"] = db.Phongs.AsQueryable().Where(g => g.TrinhTrang == true).ToList();
            ViewData["phim"] = db.Phims.AsQueryable().Where(g => g.TinhTrang == true).ToList();
            ViewBag.total = db.LichChieux.ToList().Count();
            ViewData["ca"] = db.CaChieux.AsQueryable().ToList();
            ViewData["suat"] = db.Suatchieux.AsQueryable().ToList();
            LichChieu phim = db.LichChieux.Find(id);
            return View(phim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NgayChieu,Thu,Phuthulc,IdPhim,IdCC,IdSC,GioBD,GioKT,IdPhong,TenSuat,phuthusc")] LichChieu ghe)
        {
            if (ModelState.IsValid)
            {
                if (ghe.NgayChieu < DateTime.Now)
                {
                    return RedirectToAction("Edit", "LichChieu", new { sc = 1 });
                }
                else
                {
                    var thu = ((int)ghe.NgayChieu.DayOfWeek);
                    if (thu == 6 || thu == 0)
                    {
                        ghe.Phuthulc = 30000;
                    }
                    else
                    {
                        ghe.Phuthulc = 0;
                    }
                    if (thu == 0)
                    {
                        ghe.Thu = "Chủ nhật";
                    }
                    else
                    {
                        thu = thu + 1;
                        ghe.Thu = "Thứ " + thu.ToString();
                    }

                    ghe.IdPhong = Convert.ToInt32(Request["IdPhong"]);
                    var phut = db.Phims.FirstOrDefault(p => p.Id == ghe.IdPhim).ThoiLuong;
                    var gio = phut % 60;
                    var phut1 = phut - gio * 60;
                    TimeSpan duration = new TimeSpan(gio, phut1, 00);
                    ghe.GioKT = ghe.GioBD + duration;
                    db.LichChieux.Add(ghe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(ghe);
        }
        public ActionResult Delete(int? id)
        {
            ViewData["phong"] = db.Phongs.AsQueryable().Where(g => g.TrinhTrang == true).ToList();
            ViewData["phim"] = db.Phims.AsQueryable().Where(g => g.TinhTrang == true).ToList();
            ViewBag.total = db.LichChieux.ToList().Count();
            ViewData["ca"] = db.CaChieux.AsQueryable().ToList();
            ViewData["suat"] = db.Suatchieux.AsQueryable().ToList();
            LichChieu phim = db.LichChieux.Find(id);
            return View(phim);
        }

        // POST: Admin/Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewData["phong"] = db.Phongs.AsQueryable().Where(g => g.TrinhTrang == true).ToList();
            ViewData["phim"] = db.Phims.AsQueryable().Where(g => g.TinhTrang == true).ToList();
            ViewBag.total = db.LichChieux.ToList().Count();
            ViewData["ca"] = db.CaChieux.AsQueryable().ToList();
            ViewData["suat"] = db.Suatchieux.AsQueryable().ToList();
            LichChieu phim = db.LichChieux.Find(id);
            var list = db.Ves.Where(g => g.IdLC == id).ToList();
            foreach (var item in list)
            {
                Ve v = db.Ves.Find(item.Id);
                db.Ves.Remove(v);
            }
            db.LichChieux.Remove(phim);
            db.SaveChangesAsync();
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