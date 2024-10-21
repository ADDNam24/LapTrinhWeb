using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NguyenHoangNam
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {


            routes.MapRoute(
                name: "DanhSach_",
                url: "danh-sach-thanh-vien",
                defaults: new { controller = "NguyenHoangNam", action = "DanhSach" },
                namespaces: new string[]
                {"NguyenHoangNam.Controllers"}
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "NguyenHoangNam",
                action = "Index",
                id = UrlParameter.Optional
            }
            //Thêm hàng sau để tránh xung đột giữa các controller Home
            , namespaces: new[] { "Admin.Controllers" }
            );

        }
    }
}
