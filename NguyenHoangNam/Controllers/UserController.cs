using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NguyenHoangNam.Controllers
{
    public class UserController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: User
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiachi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";

            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))

            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }

            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }

            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }

            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }

            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }

            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN; kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChi = sDiachi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                GuiMailXacNhan(sEmail, sHoTen);
                return RedirectToAction("DangNhap","DangNhap");
            }
                return this.DangKy();
        }
        public void GuiMailXacNhan(string email, string hoTen)
        {
            // Cấu hình thông tin gmail (khai báo thư viện System.Net)
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tapphongloanvu@gmail.com", "qtxt rkar buwx llid"), 
                EnableSsl = true
            };

            // Tạo email
            var message = new MailMessage();
            message.From = new MailAddress("tapphongloanvu@gmail.com"); // Thay bằng email của bạn
            message.To.Add(new MailAddress(email));
            message.Subject = "Xác nhận đăng ký thành công";
            message.Body = $"Xin chào {hoTen},\n Thông Tin Của Bạn\n{email}\n\nCảm ơn bạn đã đăng ký tài khoản tại hệ thống của chúng tôi. Chúng tôi rất vui được chào đón bạn.\n\nTrân trọng,\nNguyenHoangNam";

            // Gửi email
            mail.Send(message);
        }



    }
}