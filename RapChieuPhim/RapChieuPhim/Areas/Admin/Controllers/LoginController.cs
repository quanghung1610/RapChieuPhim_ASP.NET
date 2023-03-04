using RapChieuPhim.Dao;
using RapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(string username="", string password="")
        {
            Data d = new Data();
            TaiKhoan taiKhoan = d.TaiKhoans.FirstOrDefault(g => g.TenDangNhap == username && g.MatKhau == password);
            if(taiKhoan == null)
            {
                return RedirectToAction("Index","Login",new { check = 1 });
            }
            else
            {
                if(taiKhoan.PhanQuyen == "5")
                {
                    return RedirectToAction("Index", "Login", new { check = 2 });
                }
                else
                {
                    var newCookie = new HttpCookie("myCookieAdmin", taiKhoan.Id.ToString());
                    newCookie.Expires = DateTime.Now.AddDays(10);
                    Response.AppendCookie(newCookie);
                    Session["HoTenAdmin"] = d.ThongTins.FirstOrDefault(g => g.ThongTin_id == taiKhoan.id_ThongTin).TenNguoiDung;
                    Session["PQAdmin"] = taiKhoan.PhanQuyen;
                    return RedirectToAction("Index","Home");
                }
            }
        }
    }
}