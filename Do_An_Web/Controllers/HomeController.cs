using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
namespace Do_An_Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Header()
        {
            return PartialView();
        }
        public ActionResult Footer()
        {
            return PartialView();
        }
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(KhachHang kh)
        {
            //var list = db.KHACHHANGs.Where(n => n.TaiKhoan == kh.TaiKhoan).SingleOrDefault();
            try
            {
                db.KhachHangs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = "Tài Khoản Đã Tồn Tại";
            }
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {

            string taikhoan = f["taikhoan"].ToString();
            string matkhau = f["matkhau"].ToString();
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);

          
            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
                if (kh.Quyen.Equals("Admin"))
                {
                    return RedirectToAction("Index", "QuanLySanPham");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                } 
            }
            
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DangNhapMuaHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapMuaHang(FormCollection f)
        {

            string taikhoan = f["taikhoan"].ToString();
            string matkhau = f["matkhau"].ToString();
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
                return RedirectToAction("DatHang", "GioHang");
            }
            return View();
        }
        
    }
}