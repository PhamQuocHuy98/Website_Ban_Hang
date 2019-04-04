using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
using System.Linq.Dynamic;

namespace Do_An_Web.Controllers
{
    public class AdminController : Controller
    {
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        // GET: Admin

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ThongKe()
        {
            //Thống kê Tổng sản phẩm
            var lsp = db.SanPhams.ToList();
            ViewBag.TongSanPham = lsp.Count;

            //Thống kê Đơn đặt hàng
            int DDH = db.DonDatHangs.Count();
            ViewBag.ThongKeDonDatHang = DDH;

            //Thống kê Tổng doanh thu
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            TongDoanhThu /= 1000000;
            decimal LamTronDoanhThu = Math.Round(TongDoanhThu);
            ViewBag.TongDoanhThu = LamTronDoanhThu;

            return PartialView();
        }

        //Giải phóng biến không sài
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(db!=null)
                    db.Dispose();
               db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}