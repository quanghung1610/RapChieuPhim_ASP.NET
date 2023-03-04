using PagedList;
using RapChieuPhim.Models;
using System;
using System.Data;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class PhimsController : BaseController
    {
        private Data db = new Data();

        // GET: Admin/Phims
        public ActionResult Index(int? page, string keysearch = "")
        {
            if (page == null) page = 1;
            var books = db.Phims.OrderBy(g => g.TenPhim);
            if (!string.IsNullOrEmpty(keysearch))
            {
                books = books.Where(g => g.TenPhim.Contains(keysearch)).OrderBy(g=>g.Id);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewData["theloai"] = db.LoaiPhims.AsQueryable().ToList();
            ViewBag.total = books.Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Phims/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            return View(phim);
        }

        // GET: Admin/Phims/Create
        public ActionResult Create()
        {
            ViewData["lp"] = db.LoaiPhims.AsQueryable().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenPhim,AnhPhim,ThoiLuong,MoTa,TinhTrang,IdLoaiPhim,DienVien,DaoDien,NgayCongChieu,NgayKetThuc,NamPhatHanh,ChatLuong")] Phim phim)
        {
            HttpPostedFileBase upload = Request.Files["val-file"];

            if (upload != null && upload.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(upload.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/Upload/Image"), _FileName);
                upload.SaveAs(_path);
            }
            if (ModelState.IsValid)
            {
                var ngay = DateTime.Now;
                if (phim.NgayCongChieu <= phim.NgayKetThuc || phim.NgayCongChieu >= ngay)
                {
                    phim.AnhPhim = Path.GetFileName(upload.FileName);
                    phim.MoTa = Request["MoTa"];
                    phim.IdLoaiPhim = Request["IdLoaiPhim"];
                    db.Phims.Add(phim);
                    db.SaveChanges();
                    return RedirectToAction("Create", "Phims", new { sc = 5 });
                }
                //else if(phim.NgayCongChieu < ngay)
                //{
                //    return RedirectToAction("Create", "Phims", new { sc = 1 });
                //}
            }
            ViewBag.IdLoaiPhim = new SelectList(db.LoaiPhims, "Id", "TenLoai", phim.IdLoaiPhim);
            return View(phim);
        }

        // GET: Admin/Phims/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["lp"] = db.LoaiPhims.AsQueryable().ToList();
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdLoaiPhim = new SelectList(db.LoaiPhims, "Id", "TenLoai", phim.IdLoaiPhim);
            return View(phim);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenPhim,AnhPhim,ThoiLuong,MoTa,TinhTrang,IdLoaiPhim,DienVien,DaoDien,NgayCongChieu,NgayKetThuc,NamPhatHanh,ChatLuong")] Phim phim)
        {
            HttpPostedFileBase upload = Request.Files["val-file"];

            if (upload != null && upload.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(upload.FileName);
                string _path = Path.Combine(Server.MapPath("~/Content/Upload/Image"), _FileName);
                upload.SaveAs(_path);
            }
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    phim.AnhPhim = Path.GetFileName(upload.FileName);
                }
                else
                {
                    phim.AnhPhim = Request["AnhPhim"];
                }
                phim.ChatLuong = Request["ChatLuong"];
                phim.IdLoaiPhim = Request["IdLoaiPhim"];
                phim.MoTa = Request["MoTa"];
                db.Phims.AddOrUpdate(phim);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLoaiPhim = new SelectList(db.LoaiPhims, "Id", "TenLoai", phim.IdLoaiPhim);
            return View(phim);
        }

        // GET: Admin/Phims/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewData["lp"] = db.LoaiPhims.AsQueryable().ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim phim = db.Phims.Find(id);
            return View(phim);
        }

        // POST: Admin/Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewData["lp"] = db.LoaiPhims.AsQueryable().ToList();
            Phim phim = db.Phims.Find(id);
            var list = db.BinhLuans.Where(g => g.IdPhim == phim.Id);
            foreach (var item in list)
            {
                BinhLuan b = db.BinhLuans.Find(item.Id);
                db.BinhLuans.Remove(b);
            }
            var lc = db.LichChieux.Where(g => g.IdPhim == phim.Id).ToList();
            foreach (var item in lc)
            {
                LichChieu l = db.LichChieux.Find(item.Id);
                db.LichChieux.Remove(l);
            }
            var ve = db.Ves.Where(g => g.IdPhim == phim.Id).ToList();
            foreach (var item in ve)
            {
                Ve ve1ve1 = db.Ves.Find(item.Id);
                var dv = db.DatVes.Where(g => g.IDDatVe.Equals(item.Id)).ToList();
                foreach (var i in dv)
                {
                    DatVe d = db.DatVes.Find(i.IDDatVe);
                    db.DatVes.Remove(d);
                }
                db.Ves.Remove(ve1ve1);
            }

            db.Phims.Remove(phim);
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
