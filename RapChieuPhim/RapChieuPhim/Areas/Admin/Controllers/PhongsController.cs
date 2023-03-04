using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using RapChieuPhim.Models;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class PhongsController : BaseController
    {
        private Data db = new Data();

        // GET: Admin/Phongs
        public ActionResult Index(int? page, string keysearch = "")
        {
            if (page == null) page = 1;
            var books = db.Phongs.OrderBy(g => g.Id);
            if (!string.IsNullOrEmpty(keysearch))
            {
                books = books.Where(g => g.TenPhong.Contains(keysearch)).OrderBy(g => g.Id);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.total = db.Phongs.ToList().Count();
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
        public ActionResult Create([Bind(Include = "Id,TenPhong,SoLuong,SoLuongHang,Soluongghemoihang,TrinhTrang,MoTa")] Phong phong)
        {
            if(phong.Soluongghemoihang*phong.SoluongHang> phong.SoLuong)
            {
                return RedirectToAction("Create", "Phongs", new { sc=1 });
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Phongs.Add(phong);

                    Ghe ghe = new Ghe();
                    var m = phong.SoluongHang;
                    var n = phong.Soluongghemoihang;
                    for (int i = 1; i <= m; i++)
                    {
                        for (int j = 1; j <= n; j++)
                        {
                            ghe.Id_phong = phong.Id;
                            ghe.HangGhe = i.ToString();
                            ghe.TenGhe = char.ConvertFromUtf32(64 + i) + j;
                            ghe.TringTrang = true;
                            if (i >= m - 1)
                            {
                                ghe.Loai_id = 1;
                            }
                            else
                            {
                                ghe.Loai_id = 2;
                            }
                            db.Ghes.Add(ghe);
                            db.SaveChanges();
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(phong);
            }
            
        }

        // GET: Admin/Phongs/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Admin/Phongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenPhong,SoLuong,SoLuongHang,Soluongghemoihang,TrinhTrang,MoTa")] Phong phong)
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
            Phong phong = db.Phongs.Find(id);
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
            Phong phong = db.Phongs.Find(id);
            var list = db.Ghes.Where(g => g.Id_phong == id).ToList();
            foreach(var item in list)
            {
                Ghe g = db.Ghes.Find(item.ghe_id);
                db.Ghes.Remove(g);
                db.SaveChanges();
            }
            db.Phongs.Remove(phong);
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
