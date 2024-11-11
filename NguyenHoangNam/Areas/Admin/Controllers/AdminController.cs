using Microsoft.Ajax.Utilities;
using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        // GET: Area/DangNhap
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var sTenDN = f["TenDN"];
            var sMatKhau = f["MatKhau"];
            //Gán giá trị cho đối tượng được tạo mới (ad) 
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau
           == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;

            return RedirectToAction("Login", "Admin");
        }
    }
    
}