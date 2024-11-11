using NguyenHoangNam.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class NhaXuatBanController : BaseAdminController
    {
        // GET: Admin/NhaXuatBan
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index(int? page)
        {
            return View();
        }

        [HttpGet]
        public JsonResult DsNhaXuatBan()
        {
            try
            {
                var dsNXB = (from cd in db.NHAXUATBANs
                            select new
                            {
                                MaNXB = cd.MaNXB,
                                TenNXB = cd.TenNXB,
                                DiaChi = cd.DiaChi,
                                DienThoai = cd.DienThoai                            
                            }).ToList();
                return Json(new { code = 200, dsNXB = dsNXB, msg = "Lấy danh sách Nhà Xuất Bản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách Nhà Xuất Bản thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult Details(int maNXB)
        {
            try
            {
                var nxb = (from s in db.NHAXUATBANs
                             where (s.MaNXB == maNXB)
                             select new
                             {
                                 MaNXB = s.MaNXB,
                                 TenNXB = s.TenNXB,
                                 DiaChi = s.DiaChi,
                                 DienThoai = s.DienThoai
                             }).SingleOrDefault();
                return Json(new { code = 200, dsNXB = nxb, msg = "Lấy danh sách Nhà Xuất Bản thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách Nhà Xuất Bản thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddNhaXuatBan(string strTenNXB, string strDiaChi, string strDienThoai)
        {
            try
            {
                var nxb = new NHAXUATBAN();
                    nxb.TenNXB = strTenNXB;
                    nxb.DiaChi = strDiaChi;
                    nxb.DienThoai = strDienThoai; 
                db.NHAXUATBANs.Add(nxb);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm Nhà Xuất Bản thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm Nhà Xuất Bản thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult Update(int maNXB, string strTenNXB, string strDiaChi, string strDienThoai )
        {
            try
            {
                var unxb = db.NHAXUATBANs.SingleOrDefault(c => c.MaNXB == maNXB);
                if (unxb == null)
                {
                    return Json(new { code = 404, msg = "Không tìm thấy Nhà Xuất Bản." }, JsonRequestBehavior.AllowGet);
                }

                unxb.TenNXB = strTenNXB;
                unxb.DiaChi = strDiaChi;
                unxb.DienThoai = strDienThoai;

                db.SaveChanges();   

                return Json(new { code = 200, msg = "Sửa chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa chủ đề thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult Delete(int maNXB)
        {
            try
            {
                var cd = db.NHAXUATBANs.SingleOrDefault(c => c.MaNXB == maNXB);
                db.NHAXUATBANs.Remove(cd);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa Nhà Xuất Bản thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa Nhà Xuất Bản thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}