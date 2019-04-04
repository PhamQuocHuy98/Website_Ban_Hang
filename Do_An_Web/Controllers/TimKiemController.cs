using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An_Web.Models;
namespace Do_An_Web.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
        
        
        public ActionResult KetQuaTimKiem(string tukhoa)
        {
            List<SanPham> sanphammoi = new List<SanPham>();
            if (db.SanPhams.ToList().Count >= 0)
            {
                 sanphammoi = db.SanPhams.OrderByDescending(n => n.NgayCapNhat).Take(10).ToList();
            }
            else
            {
                sanphammoi = db.SanPhams.OrderByDescending(n => n.NgayCapNhat).ToList();
            }

            ViewBag.SanPhamMoi = sanphammoi;
            int ma;
            bool kiemtra = int.TryParse(tukhoa, out ma);
            List<SanPham> lsp = new List<SanPham>();
            if(kiemtra==false)
            {
                lsp = db.SanPhams.Where(n => n.TenSP.Contains(tukhoa)).ToList();
            }
            else
                lsp = db.SanPhams.Where(n => n.TenSP.Contains(tukhoa) || n.MaSP==ma).ToList();
            return View(lsp.OrderBy(n=>n.TenSP));
        }
    }
}