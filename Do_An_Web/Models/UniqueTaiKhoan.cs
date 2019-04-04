using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Do_An_Web.Models
{
    public class UniqueTaiKhoan : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
            var userWithTheSameAccount = db.KhachHangs.SingleOrDefault(u => u.TaiKhoan == (string)value);
            return userWithTheSameAccount == null;
        }
    }
}