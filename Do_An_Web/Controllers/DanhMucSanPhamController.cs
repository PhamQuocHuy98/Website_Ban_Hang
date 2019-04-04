using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
namespace Do_An_Web.Controllers
{
    public class DanhMucSanPhamController : Controller
    {
        // GET: DanhMucSanPham
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        
        public ActionResult HienThiSanPham(int loai)
        {
            List<SanPham> DanhSach = new List<SanPham>();
            //Điện thoại
            if (loai==1)
            {
                DanhSach = db.SanPhams.Where(n=>n.MaLoaiSP==1).ToList();
            }
            // LapTop
            else if (loai == 2)
            {
                DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 2).ToList();
            }
            // máy tính bảng
            else if(loai ==3)
            {
                DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 3).ToList();
            }
           
            return View(DanhSach);
        }
        [HttpPost]
        public ActionResult HienThiSanPham(FormCollection f,int loai, string txtGiaNho, string txtGiaLon)
        {
            List<SanPham> DanhSach = new List<SanPham>();
            if (f["SapXep"] != null)
            {
                var id = int.Parse(f["SapXep"]);
                ViewBag.SapXep = id;

               
                //Điện thoại
                if (loai == 1)
                {
                    DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 1).ToList();
                }
                // LapTop
                else if (loai == 2)
                {
                    DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 2).ToList();
                }
                // máy tính bảng
                else if (loai == 3)
                {
                    DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 3).ToList();
                }
            }
            if(txtGiaNho!=null && txtGiaLon != null)
            {
                decimal giaNho=decimal.Parse(txtGiaNho);

                decimal giaLon= decimal.Parse(txtGiaLon);
               
                
                //Điện thoại
                if (loai == 1)
                {
                    DanhSach = db.SanPhams.Where(n =>( n.MaLoaiSP==1 && (n.GiaBan >= giaNho && n.GiaBan <=giaLon))) .ToList();
                }
                // LapTop
                else if (loai == 2)
                {
                    DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.GiaBan >= giaNho && n.GiaBan <= giaLon).ToList();
                }
                // máy tính bảng
                else if (loai == 3)
                {
                    DanhSach = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.GiaBan >= (giaNho) && n.GiaBan <= (giaLon)).ToList();
                }

            }
            
            return View(DanhSach);
        }
        public ActionResult SanPhamPartial()
        {
            return PartialView();
        }
    }
}