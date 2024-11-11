using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class DonDatHangtController : BaseAdminController
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Admin/DonDatHangt
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsDonHang()
        {
            try
            {
                var ChiTietDonHang  = db.CHITIETDATHANGs.Select(ct => new {
                    MaDonHang = ct.MaDonHang,
                    MaSach = ct.MaSach,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    DONDATHANG = new {
                        MaDonHang = ct.DONDATHANG.MaDonHang,
                        DaThanhToan = ct.DONDATHANG.DaThanhToan,
                        TinhTrangGiaoHang = ct.DONDATHANG.TinhTrangGiaoHang,
                        NgayDat = ct.DONDATHANG.NgayDat,
                        NgayGiao = ct.DONDATHANG.NgayGiao,
                        MaKH = ct.DONDATHANG.MaKH
                    }
                }).ToList();

                return Json(new { code = 200, dsDonHang = ChiTietDonHang, msg = "Lấy danh sách đơn hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách đơn hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Details(int maDH)
        {
            try
            {
                var ChiTietDonHang = db.CHITIETDATHANGs.Select(ct => new {
                    MaDonHang = ct.MaDonHang,
                    MaSach = ct.MaSach,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    DONDATHANG = new
                    {
                        MaDonHang = ct.DONDATHANG.MaDonHang,
                        DaThanhToan = ct.DONDATHANG.DaThanhToan,
                        TinhTrangGiaoHang = ct.DONDATHANG.TinhTrangGiaoHang,
                        NgayDat = ct.DONDATHANG.NgayDat,
                        NgayGiao = ct.DONDATHANG.NgayGiao,
                        MaKH = ct.DONDATHANG.MaKH
                    }
                }).SingleOrDefault();

                if (ChiTietDonHang == null)
                {
                    return Json(new { code = 404, msg = "Không tìm thấy khách hàng với mã này" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { code = 200, ChiTietDonHang, msg = "Lấy thông tin khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin khách hàng thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}