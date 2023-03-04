using OfficeOpenXml;
using OfficeOpenXml.Style;
using RapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        Data db = new Data();
        // GET: Admin/ThongKe
        public ActionResult Index(string Ngay="", string TenPhong="")
        {
            if (TenPhong == "" && Ngay == "")
            {
                //ViewData["tkdt1"] = db.tkdts.AsQueryable().ToList();
                ViewData["tkdt1"] = db.tkdts.AsQueryable().ToList();
            }
            else if (Ngay != "")
            {
                var l = Convert.ToDateTime(Ngay);
                ViewData["tkdt"] = db.tkdts.AsQueryable().Where(g => g.NgayDat == l).ToList();
            }
            else if (TenPhong != "")
            {
                ViewData["tkdt"] = db.tkdts.AsQueryable().Where(g => g.TenPhong.Contains(TenPhong)).ToList();
            }
            return View();
        }
        public ActionResult Phim(string Ngay="",string TenPhim="",string Ngay1="")
        {
            if (Ngay == "" && TenPhim == ""&& Ngay1=="")
            {
                //ViewData["tkdt1"] = db.tkdttp1.AsQueryable().ToList();
                ViewData["tkdt1"] = db.tkdttp1.AsQueryable().ToList();
            }
            else if (Ngay != "" && Ngay1 !="")
            {
                var l = Convert.ToDateTime(Ngay);
                var ll = Convert.ToDateTime(Ngay1);
                var ccc = db.tkdttps.AsQueryable().Where(g => g.NgayDat >= l && g.NgayDat <= ll).GroupBy(d => d.TenPhim).Select(g=>new { TenPhim = g.Key, Tong = g.Sum(s => s.Tong)}).ToList();
                var bb = new List<tkdttp>();
                foreach (var item in ccc)
                {
                    bb.Add(new tkdttp { TenPhim = item.TenPhim, Tong = item.Tong });
                }
                ViewData["tkdt"] = bb;
            }
            else if (TenPhim != "")
            {
                ViewData["tkdt"] = db.tkdttps.AsQueryable().Where(g => g.TenPhim.Contains(TenPhim)).ToList();
            }
            return View();
        }

        public ActionResult ThongKeVe(string TenPhim = "", string SuatChieu = "")
        {
            if (TenPhim == "" && SuatChieu == "")
            {
                //ViewData["velcp1"] = db.Ve_LC_Phim.AsQueryable().ToList();
                ViewData["velcp1"] = db.Ve_LC_Phim.AsQueryable().ToList();
            }
            else if (TenPhim != null)
            {
                ViewData["velcp"] = db.Ve_LC_Phim.AsQueryable().Where(g => g.TenPhim.Contains(TenPhim)).ToList();
            }

            else if (SuatChieu != null)
            {
                ViewData["velcp"] = db.Ve_LC_Phim.AsQueryable().Where(g => g.TenSuat.Contains(SuatChieu)).ToList();
            }
            return View();
        }

        public string ExportExcel(string keySearch = "")
        {
            using (var package = new ExcelPackage())
            {
                var list = new List<tkdt>();
                list = db.tkdts.AsQueryable().ToList();
                if (list == null || !list.Any())
                {
                    return null;
                }

                var maxCol = 3;
                int iRow = 1;
                int icol = 1;

                var ws = package.Workbook.Worksheets.Add("ThongKeDoanhThuTheoThang");
                iRow = 2;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM\nĐộc Lập - Tự Do - Hạnh Phúc";
                ws.Row(2).Height = 40;

                iRow = 3;
                ws.Cells[iRow, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = "TP HCM, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                iRow = 5;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = ("THỐNG KÊ DOANH THU THEO PHÒNG").ToUpper();


                ws.Cells[1, 1, iRow, maxCol].Style.WrapText = true;
                ws.Cells[1, 1, iRow, maxCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                iRow = 9;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[iRow, 1, iRow, maxCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                icol = 1;
                ws.Column(icol).Width = 5;
                ws.Cells[iRow, icol++].Value = "STT";
                ws.Column(icol).Width = 30;
                ws.Cells[iRow, icol++].Value = "Tên phòng";
                ws.Column(icol).Width = 15;
                ws.Cells[iRow, icol++].Value = "Tổng doanh thu";
                int i = 0;
                i = i + 1;
                foreach (var item in list)
                {
                    iRow++;
                    icol = 1;
                    ws.Cells[iRow, icol++].Value = (i++).ToString();
                    ws.Cells[iRow, icol++].Value = item.TenPhong;
                    ws.Cells[iRow, icol++].Value = convert.ConvertToThousand64_From_Float(item.Tong) + "VNĐ";
                }

                // căn giữa
                ws.Cells["A10:A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // khung viền
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.WrapText = true;

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("ThongKeSinhVien_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return null;
        }

        public string ExportExcel1(string keySearch = "")
        {
            using (var package = new ExcelPackage())
            {
                var list = new List<tkdttp1>();
                list = db.tkdttp1.AsQueryable().ToList();
                if (list == null || !list.Any())
                {
                    return null;
                }

                var maxCol = 3;
                int iRow = 1;
                int icol = 1;

                var ws = package.Workbook.Worksheets.Add("ThongKeDoanhThuTheoThang");
                iRow = 2;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM\nĐộc Lập - Tự Do - Hạnh Phúc";
                ws.Row(2).Height = 40;

                iRow = 3;
                ws.Cells[iRow, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = "TP HCM, ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;

                iRow = 5;
                ws.Cells[iRow, 1, iRow, maxCol].Merge = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Value = ("THỐNG KÊ DOANH THU THEO PHIM").ToUpper();


                ws.Cells[1, 1, iRow, maxCol].Style.WrapText = true;
                ws.Cells[1, 1, iRow, maxCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells[1, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                iRow = 9;
                ws.Cells[iRow, 1, iRow, maxCol].Style.Font.Bold = true;
                ws.Cells[iRow, 1, iRow, maxCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[iRow, 1, iRow, maxCol].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                icol = 1;
                ws.Column(icol).Width = 5;
                ws.Cells[iRow, icol++].Value = "STT";
                ws.Column(icol).Width = 30;
                ws.Cells[iRow, icol++].Value = "Tên phim";
                ws.Column(icol).Width = 15;
                ws.Cells[iRow, icol++].Value = "Tổng doanh thu";
                int i = 0;
                i = i + 1;
                foreach (var item in list)
                {
                    iRow++;
                    icol = 1;
                    ws.Cells[iRow, icol++].Value = (i++).ToString();
                    ws.Cells[iRow, icol++].Value = item.TenPhim;
                    ws.Cells[iRow, icol++].Value = convert.ConvertToThousand64_From_Float(item.Tong) + "VNĐ";
                }

                // căn giữa
                ws.Cells["A10:A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // khung viền
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[9, 1, iRow, maxCol].Style.WrapText = true;

                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("ThongKeSinhVien_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return null;
        }
    }
}