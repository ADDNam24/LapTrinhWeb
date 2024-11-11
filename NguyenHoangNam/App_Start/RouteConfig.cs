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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Trang chu",
                url: "",
                defaults: new { controller = "NguyenHoangNam", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Sach theo Chu de",
            url: "sach-theo-chu-de-{MaCD}",
            defaults: new { controller = "NguyenHoangNam", action = "SachTheoChuDe" },
            namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );


            routes.MapRoute(
                name: "Sach theo NXB",
                url: "sach-theo-nxb-{MaNXB}",
                defaults: new { controller = "NguyenHoangNam", action = "SachTheoNhaXuatBan", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Chi tiet sach",
                url: "chi-tiet-sach-{MaSach}",
                defaults: new { controller = "NguyenHoangNam", action = "BookDetail", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Dang ky",
                url: "dang-ky",
                defaults: new { controller = "User", action = "DangKy" },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Dang nhap",
                url: "dang-nhap",
                defaults: new { controller = "DangNhap", action = "DangNhap", url = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Gio hang",
                url: "gio-hang",
                defaults: new { controller = "GioHang", action = "GioHang" },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Dat hang",
                url: "dat-hang",
                defaults: new { controller = "GioHang", action = "DatHang" },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Trang tin",
                url: "{metatitle}",
                defaults: new { controller = "NguyenHoangNam", action = "TrangTin", metatitle = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "NguyenHoangNam.Controllers" }
            );
        }
    }
}
