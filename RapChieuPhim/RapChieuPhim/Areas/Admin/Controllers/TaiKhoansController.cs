using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RapChieuPhim.Models;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class TaiKhoansController : BaseController
    {
        private Data db = new Data();

        // GET: Admin/TaiKhoans
        public ActionResult Index()
        {
            var taiKhoans = db.TaiKhoans.Include(t => t.ThongTin);
            return View(taiKhoans.ToList());
        }
        // POST: Admin/TaiKhoans/GetJsonResult
        [HttpPost]
        public JsonResult GetJsonResult()
        {
            List<TaiKhoan> taiKhoans = db.TaiKhoans.Include(t => t.ThongTin).Where(tk => tk.PhanQuyen != "5").ToList();
            var jsonNhanVien = taiKhoans.Select(tk => new
            {
                MaKH = tk.Id,
                TenKH = tk.ThongTin.TenNguoiDung,
                DiaChi = tk.ThongTin.DiaChi,
                NgaySinh = tk.ThongTin.NgaySinh,
                GioiTinh = tk.ThongTin.GioiTinh,
                NgayDK = tk.NgayDangKy,
                PhanQuyen = tk.PhanQuyen,
                TrinhTrang = tk.TinhTrang,
            });
            return Json(jsonNhanVien, behavior: JsonRequestBehavior.AllowGet);
        }
        // POST: Admin/TaiKhoans/ActivityUser
        [HttpPost]
        public bool ActivityUser(int? id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            try
            {
                if (taiKhoan!=null)
                {
                    taiKhoan.TinhTrang = false;

                }
                else
                {
                    taiKhoan.TinhTrang = true;
                }
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // POST: Admin/TaiKhoans/changeRole
        [HttpPost]
        public bool changeRole(int? id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            try
            {
                if (taiKhoan.PhanQuyen == "MANAGA")
                {
                    taiKhoan.PhanQuyen = "ADMIN";

                }
                else
                {
                    taiKhoan.PhanQuyen = "MANAGA";
                }
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        // GET: Admin/TaiKhoans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThongTin_id,TenNguoiDung,DiaChi,GioiTinh,NgaySinh,Email")] ThongTin taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.ThongTins.Add(taiKhoan);
                TaiKhoan v = new TaiKhoan();
                v.id_ThongTin = taiKhoan.ThongTin_id;
                v.TenDangNhap = taiKhoan.Email;
                v.MatKhau = Request["MatKhau"];
                v.NgayDangKy = DateTime.Now;
                v.TinhTrang = true;
                v.PhanQuyen = Request["PhanQuyen"];
                db.TaiKhoans.Add(v);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_ThongTin = new SelectList(db.ThongTins, "ThongTin_id", "TenNguoiDung", taiKhoan.id_ThongTin);
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenDangNhap,MatKhau,NgayDangKy,TinhTrang,PhanQuyen,id_ThongTin")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_ThongTin = new SelectList(db.ThongTins, "ThongTin_id", "TenNguoiDung", taiKhoan.id_ThongTin);
            return View(taiKhoan);
        }

        // GET: Admin/TaiKhoans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            return View(taiKhoan);
        }

        // POST: Admin/TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
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
        public ActionResult Logout(string username, string email, string password)
        {

            Session.Clear();
            if (Request.Cookies["myCookieAdmin"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookieAdmin"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return Redirect("/Admin/Login");

        }
    }
}
