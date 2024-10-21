using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHoangNam.Controllers
{
    public class DangNhapController : Controller
    {
        // GET: DangNhap
        SachOnlineEntities db = new SachOnlineEntities();
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        // GET: Area/DangNhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatkhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatkhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {

                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatkhau);
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatkhau);
                if (ad != null)
                {
                    Session["Admin"] = ad;
                    Session["TaiKhoan"] = ad;
                    return RedirectToAction("Index", "Admin", new {area = "Admin"});
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    Session["TenKH"] = kh.HoTen;
                    
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatkhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                }
                else
                {

                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    Session["TaiKhoan"] = kh;


                }
            }
            
   
            return RedirectToAction("Index","NguyenHoangNam");
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            Session["TenKH"] = null;

            return RedirectToAction("Index","NguyenHoangNam");
        }
    }

    }