using NguyenHoangNam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenHoangNam.Areas.Admin.Controllers
{
    public class TrangTinController : BaseAdminController
    {
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View(db.TRANGTINs.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TRANGTIN tt)
        {
            if (ModelState.IsValid)
            {
                tt.MetaTitle = tt.TenTrang.RemoveDiacritics().Replace(" ", "-");
                tt.NgayTao = DateTime.Now;
              
                db.TRANGTINs.Add(tt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var tt = db.TRANGTINs.Where(t => t.MaTT == id);
            return View(tt.SingleOrDefault());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            int matt = int.Parse(f["MaTT"]);
            if (ModelState.IsValid)
            {
                var tt = db.TRANGTINs.Where(t => t.MaTT == matt).SingleOrDefault();
                tt.TenTrang = f["TenTrang"];
                tt.NoiDung = f["NoiDung"];
                tt.NgayTao = Convert.ToDateTime(f["NgayTao"]);
                tt.MetaTitle = f["TenTrang"].RemoveDiacritics().Replace(" ", "-");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var tt = from t in db.TRANGTINs where t.MaTT == id select t;
            return View(tt.SingleOrDefault());
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var tt = (from t in db.TRANGTINs where t.MaTT == id select t).SingleOrDefault();
            db.TRANGTINs.Remove(tt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}