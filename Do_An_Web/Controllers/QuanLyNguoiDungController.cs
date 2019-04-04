using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Do_An_Web.Models;
using System.Activities.Statements;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;

namespace Do_An_Web.Controllers
{
    public class QuanLyNguoiDungController : Controller
    {
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LayDSKhachHang()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            //Kích cỡ khi phân trang (10,20,50,100)    
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // Lấy toàn bộ dữ liệu trong bảng KhachHangs 
            var DLKhachHang = (from TempKhachHang in db.KhachHangs
                                select TempKhachHang);
            //Lọc  
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                DLKhachHang = DLKhachHang.OrderBy(sortColumn + " " + sortColumnDir);
            }

            var sortedList = DLKhachHang.OrderBy(obj => obj.MaKH).ThenByDescending(obj => obj.MaKH).ToList();
            //Tìm kiếm theo Mã Khách hàng / Họ Tên / Tài khoản / SĐT
            if (!string.IsNullOrEmpty(searchValue))
            {
                DLKhachHang = DLKhachHang.Where(m => m.HoTen.Contains(searchValue) || m.TaiKhoan.Contains(searchValue) || m.DienThoai.Contains(searchValue) || m.MaKH.ToString() == searchValue);
            }

            //Đếm bao nhiêu dòng     
            recordsTotal = DLKhachHang.Count();

            //Phân trang   
            var data = DLKhachHang.Skip(skip).Take(pageSize).ToList();

            
            //Trả về Dữ liệu Json
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult TaoMoi(KhachHang kh)
        {
            if(!ModelState.IsValid)
            {
               return View("TaoMoi",kh);
            }
            db.KhachHangs.Add(kh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            if(id==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            if(kh==null)
            {
                return HttpNotFound();

            }
            return View(kh);
        }

        [HttpPost]
        public JsonResult CheckUsername(string taikhoan, int? MaKH)
        {
            var tk = db.KhachHangs.Find(MaKH);
            if(tk.TaiKhoan.ToString()==taikhoan)
            {
                return Json(true);
            }
            bool isValid = !db.KhachHangs.ToList().Exists(p => p.TaiKhoan.Equals(taikhoan, StringComparison.CurrentCultureIgnoreCase));
            return Json(isValid);
        }

        [HttpPost]
        public JsonResult CheckEmail(string Email, int? MaKH)
        {
            var tk = db.KhachHangs.Find(MaKH);
            if (tk.Email.ToString() == Email)
            {
                return Json(true);
            }
            bool isValid = !db.KhachHangs.ToList().Exists(p => p.Email.Equals(Email, StringComparison.CurrentCultureIgnoreCase));
            return Json(isValid);
        }

        [HttpPost]
        public ActionResult ChinhSua(KhachHang model, int id)
        {

            //KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.MaKH == id);
            //model.Email = kh.Email;
            //model.TaiKhoan = kh.TaiKhoan;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(model).State = EntityState.Modified;
                  //  db.Set<KhachHang>().AddOrUpdate(model);
                    db.SaveChanges();
                }
                catch 
                {
                    ViewBag.Loi = "Lỗi cập nhật dữ liệu!";
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Xoa(int? id)
        {
            var MaKH = db.KhachHangs.Find(id);
            if(MaKH==null)
            {
                return Json(data: "Không thể xoá!", behavior: JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {
                    db.KhachHangs.Remove(MaKH);
                    db.SaveChanges();
                    return Json(data: "Đã xoá!", behavior: JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(data: "Lỗi SQL", behavior: JsonRequestBehavior.AllowGet);
                }
                
            }
        }
    }
}