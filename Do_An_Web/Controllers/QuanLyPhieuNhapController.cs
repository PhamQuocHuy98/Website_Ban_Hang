using Do_An_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An_Web.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        [HttpGet]
        public ActionResult NhapHang()
        {
            //ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
           // ViewBag.DSSanPham = db.SanPhams;
            ViewBag.DSSanPham = new SelectList(db.SanPhams.OrderBy(n => n.TenSP), "MaSP", "TenSP");
            return View();
        }

        [HttpPost]
        public ActionResult NhapHang(PhieuNhap ph, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            
            ViewBag.DSSanPham = new SelectList(db.SanPhams.OrderBy(n => n.TenSP), "MaSP", "TenSP");
            
            db.PhieuNhaps.Add(ph);
            db.SaveChanges();

            //SaveChange để lấy mã phiếu nhập gán cho lstChiTietPhieuNhap
            SanPham sp;
            foreach(var item in lstModel)
            {
                //Cập nhật lại số lượng tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuong;
                //Gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
                item.MaPN = ph.MaPN;
            }
            try
            {
                db.ChiTietPhieuNhaps.AddRange(lstModel);
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return View();
        }
    }
}