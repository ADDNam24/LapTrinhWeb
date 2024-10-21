using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text; 

namespace NguyenHoangNam.Controllers
{
    public class HoangNamGioHangController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;

        }
        public ActionResult ThemGioHang(int id, string url)
        {
            List<GioHang> lstgioHangs = LayGioHang();
            GioHang sp = lstgioHangs.Find(n=>n.iMaSach == id);
            if (sp == null)
            {
                sp = new GioHang(id);
                lstgioHangs.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if( lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n=>n.iSoLuong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);

            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang =LayGioHang();
            if( lstGioHang.Count == 0)
            {
                return RedirectToAction("Index","NguyenHoangNam");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView("_GioHangPartial");
        }
        public ActionResult XoaSPKHoiGioHang(int iMaSach)
        {
            List<GioHang> listGioHang = LayGioHang();
            GioHang sp = listGioHang.SingleOrDefault(n=>n.iMaSach == iMaSach);
            if(sp != null)
            {
                listGioHang.RemoveAll(n=>n.iMaSach==iMaSach);
                if(listGioHang.Count ==0)
                {
                    return RedirectToAction("Index", "NguyenHoangNam");
                }
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhatGioHang(int IMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == IMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "NguyenHoangNam");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "DangNhap");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "NguyenHoangNam");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang(); 
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = string.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG
                {
                    MaDonHang = ddh.MaDonHang,
                    MaSach = item.iMaSach,
                    SoLuong = item.iSoLuong,
                    DonGia = (decimal)item.dDonGia
                };
                db.CHITIETDATHANGs.Add(ctdh);
            }
            db.SaveChanges();

            List<GioHang> gioHangItems = lstGioHang.Select(item => new GioHang(item.iMaSach)
            {
                iSoLuong = item.iSoLuong
            }).ToList();

            int tongSoLuong = gioHangItems.Sum(x => x.iSoLuong);
            decimal tongTien = (decimal)gioHangItems.Sum(x => x.dThanhTien);
            GuiMailXacNhan(kh.Email, kh.HoTen, ddh.NgayGiao, gioHangItems, tongSoLuong, tongTien);
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "HoangNamGioHang");
        }


        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public void GuiMailXacNhan(string email, string hoTen, DateTime? ngaygiao, List<GioHang> gioHangItems, int tongSoLuong, decimal tongTien)
        {
            // Cấu hình thông tin gmail (khai báo thư viện System.Net)
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tapphongloanvu@gmail.com", "qtxt rkar buwx llid"),
                EnableSsl = true
            };

            // Tạo email
            var message = new MailMessage
            {
                From = new MailAddress("tapphongloanvu@gmail.com"), // Thay bằng email của bạn
                Subject = "Xác nhận đặt hàng thành công!",
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));

            // Tạo bảng HTML
            var body = new StringBuilder();
            body.AppendLine($"<p>ĐƠN HÀNG CỦA <b>{hoTen}</b> ĐÃ ĐƯỢC CHÚNG TÔI GHI NHẬN, chúng tôi sẽ liên hệ sau và giao hàng đúng hẹn.</p>");
            body.AppendLine($"<p>Dự kiến ngày giao hàng: <b>{ngaygiao?.ToString("dd/MM/yyyy")}</b></p>");
            body.AppendLine("<table align='center' border='1' style='width:100%;'>");
            body.AppendLine("<tr style='text-align:center; font-weight:bold;'>");
            body.AppendLine("<td>Mã Sách</td><td>Tên Sách</td><td>Ảnh Bìa</td><td>Số Lượng</td><td>Đơn Giá</td><td>Thành Tiền</td>");
            body.AppendLine("</tr>");

            foreach (var item in gioHangItems)
            {
                // Lấy thông tin sách từ cơ sở dữ liệu dựa trên mã sách
                SACH sach = db.SACHes.Single(n => n.MaSach == item.iMaSach); // Lấy thông tin sách
                var imagePath = $"https://yourdomain.com/Images/{sach.AnhBia}"; // Đường dẫn tuyệt đối tới ảnh bìa

                body.AppendLine("<tr style='text-align:center;'>");
                body.AppendLine($"<td>{item.iMaSach}</td>");
                body.AppendLine($"<td>{sach.TenSach}</td>"); // Tên sách từ cơ sở dữ liệu
                body.AppendLine($"<td><img src='{imagePath}' style='width:100px;' /></td>"); // Đường dẫn ảnh bìa sách
                body.AppendLine($"<td>{item.iSoLuong}</td>");
                body.AppendLine($"<td>{item.dDonGia:#,##0,0} VND</td>");
                body.AppendLine($"<td>{item.dThanhTien:#,##0,0} VND</td>");
                body.AppendLine("</tr>");
            }

            body.AppendLine("<tr>");
            body.AppendLine($"<td colspan='3' style='text-align:right; color:red; font-weight:bold;'>Số lượng sách: {tongSoLuong:#,##0}</td>");
            body.AppendLine($"<td colspan='3' style='text-align:center; color:red; font-weight:bold;'>Tổng tiền: {tongTien:#,##0,0} VNĐ</td>");
            body.AppendLine("</tr>");
            body.AppendLine("</table>");

            body.AppendLine("<p>Trân trọng,<br/>NguyenHoangNam</p>");

            message.Body = body.ToString();

            // Gửi email
            mail.Send(message);
        }


    }


}