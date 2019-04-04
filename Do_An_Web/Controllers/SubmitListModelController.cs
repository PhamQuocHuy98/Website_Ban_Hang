using Do_An_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An_Web.Controllers
{
    public class SubmitListModelController : Controller
    {
        // GET: SubmitListModel
        public ActionResult Index(IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            return View();
        }
    }
}