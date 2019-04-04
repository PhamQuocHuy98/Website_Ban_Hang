using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Do_An_Web.Models
{
    public class ItemGioHang
    {
        
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string HinhAnh { get; set; }
        public decimal ThanhTien { get; set; }

        public ItemGioHang()
        {

        }
        public ItemGioHang(int MaSP)
        {

            using (dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities())
            {
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Where(n => n.MaSP == MaSP).SingleOrDefault();
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.GiaBan.Value;
                this.SoLuong = 1;
                this.ThanhTien = DonGia * SoLuong;
            }
        }
        public ItemGioHang(int MaSP,int SL)
        {
            
            using (dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities())
            {
                this.MaSP = MaSP;
                SanPham sp = db.SanPhams.Where(n => n.MaSP == MaSP).SingleOrDefault();
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.GiaBan.Value;
                this.SoLuong = SL;
                this.ThanhTien = DonGia * SoLuong;
            }
        }

    }
}