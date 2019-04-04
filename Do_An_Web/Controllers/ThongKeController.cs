using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;

namespace Do_An_Web.Controllers
{
    public class ThongKeController : Controller
    {
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        // GET: ThongKe
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString(); //Lấy số lượng người truy cập từ Application
            ViewBag.SoNguoiDangTrucTuyen = HttpContext.Application["SoNguoiDangTrucTuyen"].ToString();
            
            //Thống kê Tổng người dùng
            var lnd = db.KhachHangs.ToList();
            ViewBag.TongNguoiDUng = lnd.Count;

            ViewBag.T1 = LayDoanhThuTungThang(1);
            ViewBag.T2 = LayDoanhThuTungThang(2);
            ViewBag.T3 = LayDoanhThuTungThang(3);
            ViewBag.T4 = LayDoanhThuTungThang(4);
            ViewBag.T5 = LayDoanhThuTungThang(5);
            ViewBag.T6 = LayDoanhThuTungThang(6);
            ViewBag.T7 = LayDoanhThuTungThang(7);
            ViewBag.T8 = LayDoanhThuTungThang(8);
            ViewBag.T9 = LayDoanhThuTungThang(9);
            ViewBag.T10 = LayDoanhThuTungThang(10);
            ViewBag.T11 = LayDoanhThuTungThang(11);
            ViewBag.T12 = LayDoanhThuTungThang(12);

            return View();
        }

        public Decimal TongDoanhThuTheoThangNam(int Thang, int Nam)
        {
            //List ra những đơn đặt hàng nào có tháng,năm tương ứng
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            //Duyệt chi tiết của đơn đặt hàng đó và lấy tổng tiền của tất cả các sản phẩm thuộc đơn hàng đó
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;
        }

        public decimal LayDoanhThuTungThang(int Thang)
        {
            decimal DoanhThu = 0;
            var lstDT= db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang);
            foreach (var item in lstDT)
            {
                DoanhThu += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return DoanhThu;
        }

        public ActionResult BieuDo()
        {
            
            

            return View();
        }

       
    }
}