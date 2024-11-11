using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class MenuController : BaseAdminController
    {
        SachOnlineEntities data = new SachOnlineEntities();
        // GET: Admin/Menu
        public ActionResult Index()

        {

            var listhenu = data.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
            int[] a = new int[listhenu.Count()];

            for (int i = 0; i < listhenu.Count; i++)

            {

                int id = listhenu[i].Id;

                var l = data.MENUs.Where(m => m.ParentId == id);

                int k = l.Count();

                a[i] = k;
            }
            ViewBag.lst = a;

            List<CHUDE> cd = data.CHUDEs.ToList();

            ViewBag.ChuDe = cd;

            List<TRANGTIN> tt = data.TRANGTINs.ToList();

            ViewBag.TrangTin = tt;

            return View(listhenu);
        }

        [ChildActionOnly]
        public ActionResult CHildMenu(int parentId)

        {

            List<MENU> lst = new List<MENU>();

            lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();

            ViewBag.Count = lst.Count();

            int[] a = new int[lst.Count()];

            for (int i = 0; i < lst.Count; i++)
            {
                int id = lst[i].Id;

                var l = data.MENUs.Where(m => m.ParentId == id);

                int k = l.Count();

                a[i] = k;

            }
            ViewBag.lst = a;

            return PartialView("ChildMenu", lst);
        }

        [ChildActionOnly]
        public ActionResult ChildMenu1(int parentId)
        {
            List<MENU> lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count;
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count(); i++)
            {
                int id = lst[i].Id;
                var l = data.MENUs.Where(m => m.ParentId == id).ToList();
                int k = l.Count();
                a[i] = k;
            }
            ViewBag.lst = a;
            return PartialView("ChildMenu1", lst);
        }

        [HttpPost]
        public ActionResult AddMenu(FormCollection f)

        {

            if (!String.IsNullOrEmpty(f["ThemChuDe"]))
            {
                MENU m = new MENU();
                int maCD = int.Parse(f["MaCD"].ToString());
                var cd = data.CHUDEs.Where(c => c.MaCD == maCD).SingleOrDefault();
                m.MenuName = cd.TenChuDe;
                m.MenuLink = "sach-theo-chu-de-" + cd.MaCD;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number"]);
                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = data.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();
                }
                else
                {
                    l = data.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count(); i++)
                {
                    l[i].OrderNumber++;
                }
                data.MENUs.Add(m);

                data.SaveChanges();
            }
            else if (!String.IsNullOrEmpty(f["ThemTrang"]))
            {
                MENU m = new MENU();
                int maTT = int.Parse(f["MaTT"].ToString());
                var trang = data.TRANGTINs.Where(t => t.MaTT == maTT).SingleOrDefault();
                m.MenuName = trang.TenTrang;
                m.MenuLink = trang.MetaTitle;
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["parentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number1"]);

                List<MENU> l = null;

                if (m.ParentId == null)
                {
                    l = data.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();
                }
                else
                {
                    l = data.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count(); i++)
                {
                    l[i].OrderNumber++;
                }
                data.MENUs.Add(m);
                data.SaveChanges();
            }
            else if (!String.IsNullOrEmpty(f["ThemLink"]))
            {
                MENU m = new MENU();
                m.MenuName = f["TenMenu"];
                m.MenuLink = f["Link"];
                if (!String.IsNullOrEmpty(f["ParentID"]))
                {
                    m.ParentId = int.Parse(f["ParentID"]);
                }
                else
                {
                    m.ParentId = null;
                }
                m.OrderNumber = int.Parse(f["Number2"]);
                List<MENU> l = null;
                if (m.ParentId == null)
                {
                    l = data.MENUs.Where(k => k.ParentId == null && k.OrderNumber >= m.OrderNumber).ToList();
                }
                else
                {
                    l = data.MENUs.Where(k => k.ParentId == m.ParentId && k.OrderNumber >= m.OrderNumber).ToList();
                }
                for (int i = 0; i < l.Count(); i++)
                {
                    l[1].OrderNumber++;
                }
                data.MENUs.Add(m);
                data.SaveChanges();
            }
                return Redirect("~/Admin/Menu/Index");
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            List<MENU> submn = data.MENUs.Where(m => m.ParentId == id).ToList();
            if (submn.Count() > 0)
            {
                return Json(new { code = 500, msg = "Còn menu con, không xóa được." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var mn = data.MENUs.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if (mn.ParentId == null)
                {
                    l = data.MENUs.Where(k => k.ParentId == null && k.OrderNumber > mn.OrderNumber).ToList();
                }
                else
                {
                    l = data.MENUs.Where(k => k.ParentId == mn.ParentId && k.OrderNumber > mn.OrderNumber).ToList(); 
                }
                for (int i = 0; i < l.Count; i++)
                {
                    l[i].OrderNumber--;
                }
                data.MENUs.Remove(mn);

                data.SaveChanges();

                return Json(new { code = 200, msg = "Xóa thành công." }, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpGet]
        public JsonResult Update(int id)
        {
            try
            {
                var mn = (from m in data.MENUs
                          where (m.Id == id)
                          select new
                          {
                              Id = m.Id,
                              MenuName = m.MenuName,
                              MenuLink = m.MenuLink,
                              OrderNumber = m.OrderNumber

                          }).SingleOrDefault();
                return Json(new { code = 200, mn = mn, msg = "Lấy thông tin thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public JsonResult Update(int id, string strTenMenu, string strlink, int STT)
        {
            try
            {
                var mn = data.MENUs.SingleOrDefault(m => m.Id == id);
                List<MENU> l = null;
                if (STT < mn.OrderNumber)
                {
                    if (mn.ParentId == null)
                    {
                        l = data.MENUs.Where(m => m.ParentId == null && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    }
                    else
                    {
                        l = data.MENUs.Where(m => m.ParentId == mn.ParentId && m.OrderNumber >= STT && m.OrderNumber < mn.OrderNumber).ToList();
                    }
                    for (int i = 0; i < l.Count(); i++)
                    {
                        l[i].OrderNumber++;
                    }
                }

                else if (STT > mn.OrderNumber)
                {
                    if (mn.ParentId == null)
                    {
                        l = data.MENUs.Where(m => m.ParentId == null && mn.OrderNumber > mn.OrderNumber && m.OrderNumber <= STT).ToList();
                    }
                    else
                    {
                        l = data.MENUs.Where(m => m.ParentId == mn.ParentId && m.OrderNumber > mn.OrderNumber && m.OrderNumber <= STT).ToList();
                    }
                }
                mn.MenuName = strTenMenu;
                mn.MenuLink = strlink;
                mn.OrderNumber = STT;
                data.SaveChanges();
                return Json(new { code = 200, msg = "Sửa menu thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new{code = 500, msg = "sửa menu thất bại. Lỗi" + ex.Message }, JsonRequestBehavior. AllowGet);
            }
           }
        }
            
    } 
