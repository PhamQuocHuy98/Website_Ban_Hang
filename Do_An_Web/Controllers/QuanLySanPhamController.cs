using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using Do_An_Web.Models;
using System.Data.Entity.Migrations;

namespace Do_An_Web.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LayDSSanPham()
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
            var DLSanPham = (from DLSanPhams in db.SanPhams
                                select DLSanPhams);
            //Lọc    
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                DLSanPham= DLSanPham.OrderBy(sortColumn + " " + sortColumnDir);
            }
            //Tìm kiếm   
            if (!string.IsNullOrEmpty(searchValue))
            {
                DLSanPham = DLSanPham.Where(m => m.TenSP.Contains(searchValue) || m.MaSP.ToString()==searchValue);
            }

            //Đếm bao nhiêu dòng     
            recordsTotal = DLSanPham.Count();
            
            //Phân trang   
            var data = DLSanPham.Skip(skip).Take(pageSize).ToList();
            
            //Trả về Dữ liệu Json
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load DropDownList Nhà cung cấp và Loại Sản phẩm
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSanPham), "MaLoaiSanPham", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase HinhAnh)
        {
            //Load DropDownList Nhà cung cấp và Loại Sản phẩm
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSanPham), "MaLoaiSanPham", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //Kiểm tra Hình có tồn tại hay chưa
            if (HinhAnh.ContentLength > 0)
            {
                //Lấy tên Hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //Lấy Hình ảnh chuyển vào Thư mục Hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                //Nếu hình ảnh có trong thư mục thì báo lỗi
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload = "Hình đã tồn tại";
                    return View();
                }
                else
                {
                    //Tiến hành lấy hình ảnh đưa vào mục Content/Images
                    HinhAnh.SaveAs(path);
                    sp.HinhAnh = fileName;
                }
            }
            sp.NgayCapNhat = DateTime.Now;
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào ID
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            //Load DropDownList Nhà cung cấp và Loại Sản phẩm
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSanPham), "MaLoaiSanPham", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase HinhAnh, int id)
        {
            //Load DropDownList Nhà cung cấp và Loại Sản phẩm
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSanPham), "MaLoaiSanPham", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            
            if (HinhAnh !=null && HinhAnh.ContentLength != 0)
            {
                //Lấy tên Hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //Lấy Hình ảnh chuyển vào Thư mục Hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                //Nếu hình ảnh có trong thư mục thì báo lỗi
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Upload = "Hình đã tồn tại";
                    return View(model);
                }
                else
                {
                    //Tiến hành lấy hình ảnh đưa vào mục Content/Images
                    HinhAnh.SaveAs(path);
                    model.HinhAnh = fileName;
                }
            }

            if (HinhAnh == null)
            {
                SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
                model.HinhAnh = sp.HinhAnh;
            }

            model.NgayCapNhat = DateTime.Now;
            if (ModelState.IsValid)
            {
               
                try
                {
                    //db.Entry(model).State = EntityState.Modified;
                    db.Set<SanPham>().AddOrUpdate(model);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var errors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in errors.ValidationErrors)
                        {
                            // get the error message 
                            string errorMessage = validationError.ErrorMessage;
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Xoa(int? id)
        {
            var MaSP = db.SanPhams.Find(id);
            if (MaSP == null)
            {
                return Json(data: "Không thể xoá!", behavior: JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.SanPhams.Remove(MaSP);
                db.SaveChanges();
                return Json(data: "Đã xoá!", behavior: JsonRequestBehavior.AllowGet);
            }
        }
    }
}

