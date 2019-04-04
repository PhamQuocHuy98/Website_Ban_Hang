using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Do_An_Web.Models;
namespace Do_An_Web.Models
{
    public class UniqueEmail : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            dboQuanLyBanHangEntities db = new dboQuanLyBanHangEntities();
            var userWithTheSameEmail = db.KhachHangs.SingleOrDefault(u => u.Email == (string)value);
            return userWithTheSameEmail == null;
        }
    }
}