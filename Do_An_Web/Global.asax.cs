using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Do_An_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Khởi tạo Application để lưu lượt truy cập vào Website
            //PageView: Đếm số lượt truy cập của Website
            //Online: Đếm số lượt người hiện đang Online
            Application["SoNguoiTruyCap"] = 0;
            Application["SoNguoiDangTrucTuyen"] = 0;
        }
        
        protected void Session_Start()
        {
            Application.Lock(); //Dùng để đồng bộ hoá, sử lý theo trình tự nếu lượt truy cập cùng lúc
            Application["SoNguoiTruyCap"] = (int)Application["SoNguoiTruyCap"] + 1;
            Application["SoNguoiDangTrucTuyen"] = (int)Application["SoNguoiDangTrucTuyen"] + 1;
            Application.UnLock();
        }
        protected void Session_End()
        {
            Application.Lock(); //Dùng để đồng bộ hoá, sử lý theo trình tự nếu lượt truy cập cùng lúc
            Application["SoNguoiDangTrucTuyen"] = (int)Application["SoNguoiDangTrucTuyen"] - 1;
            Application.UnLock();
        }
    }
}
