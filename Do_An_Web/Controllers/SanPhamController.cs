using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
namespace Do_An_Web.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        // Hiển Thị 4 Sản Phẩm
        public ActionResult SanPham()
        {
            var list = db.SanPhams.OrderBy(n => n.NgayCapNhat);
            ViewBag.LstSPMoi = list;

            // lấy 3 sản phẩm mới nhất thuộc 3 loại khác nhau
            var listloaisp = db.SanPhams.ToList();
            
            ViewBag.LstLSP = listloaisp;
            return View();
        }
        
        public ActionResult HienThiSanPham()
        {
           
            return PartialView();
        }

        // Trang chi tiết sản phẩm
        public ActionResult XemChiTiet(int? id)
        {
            // Kiểm tra id truyền vào có null hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            // nếu không có sản phẩm nào có id đã truyền vào
            if (sp == null)
            {
               return HttpNotFound();
            }

            return View(sp);
        }
        // lấy 4 sản phẩm cùng loại hiển thị lên trang chi tiết sản phẩm
        public ActionResult LaySanPhamCungLoai(int maloai)
        {

            var list = db.SanPhams.Where(n => n.MaLoaiSP == maloai).Take(4);
            return View(list);
        }
    }
}