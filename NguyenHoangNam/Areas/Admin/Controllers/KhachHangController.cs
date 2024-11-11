using NguyenHoangNam.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class KhachHangController : BaseAdminController 
    {
        SachOnlineEntities db = new SachOnlineEntities();

        // GET: Admin/DonDatHang
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult DsKhachHang()
        {
            try
            {
                var dsKH = db.KHACHHANGs.Select(kh => new {
                    MaKH = kh.MaKH,
                    HoTen = kh.HoTen,
                    TaiKhoan = kh.TaiKhoan,
                    Email = kh.Email,
                    DienThoai = kh.DienThoai
                }).ToList();
                return Json(new { code = 200, dsKH, msg = "Lấy danh sách khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Details(int maKH)
        {
            try
            {
                var kh = db.KHACHHANGs.Where(k => k.MaKH == maKH).Select(kh1 => new {
                    kh1.MaKH,
                    kh1.HoTen,
                    kh1.TaiKhoan,
                    kh1.MatKhau,
                    kh1.Email,
                    kh1.DiaChi,
                    kh1.DienThoai,
                    kh1.NgaySinh
                }).SingleOrDefault();
                return Json(new { code = 200, kh, msg = "Lấy thông tin khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddKhachHang(KHACHHANG khachHang)
        {
            try
            {
                db.KHACHHANGs.Add(khachHang);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateKhachHang(KHACHHANG khachHang)
        {
            try
            {
                var kh = db.KHACHHANGs.SingleOrDefault(k => k.MaKH == khachHang.MaKH);
                if (kh != null)
                {
                    kh.HoTen = khachHang.HoTen;
                    kh.TaiKhoan = khachHang.TaiKhoan;
                    kh.MatKhau = khachHang.MatKhau;
                    kh.Email = khachHang.Email;
                    kh.DiaChi = khachHang.DiaChi;
                    kh.DienThoai = khachHang.DienThoai;
                    kh.NgaySinh = khachHang.NgaySinh;
                    db.SaveChanges();
                }
                return Json(new { code = 200, msg = "Cập nhật khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteKhachHang(int maKH)
        {
            try
            {
                var kh = db.KHACHHANGs.SingleOrDefault(k => k.MaKH == maKH);
                db.KHACHHANGs.Remove(kh);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}