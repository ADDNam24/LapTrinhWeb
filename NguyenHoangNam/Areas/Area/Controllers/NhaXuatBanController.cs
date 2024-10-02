using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenHoangNam.Models;

namespace NguyenHoangNam.Areas.Area.Controllers
{
    public class NhaXuatBanController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Area/NhaXuatBan
        public ActionResult Index()
        {
            var data = db.NHAXUATBANs.ToList();
            return View(data);
        }

        //Chi tiết nhà xuất bản

        public ActionResult Detail(int id)
        {
            //int maNXB = int.Parse(Request.QueryString["id"]);
            var kq = db.NHAXUATBANs.Where(nxb=>nxb.MaNXB == id).SingleOrDefault();
            return View(kq);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection f)
        {
            NHAXUATBAN nxb = new NHAXUATBAN();
            nxb.TenNXB = f["TenNXB"];
            nxb.DiaChi = f["DiaChi"];
            nxb.DienThoai = f["DienThoai"];

            db.NHAXUATBANs.Add(nxb);
            db.SaveChanges(); 

            return View("Create");
        }

        [HttpPost]

        public ActionResult Luu1(NHAXUATBAN nxb)
        {
           
            db.NHAXUATBANs.Add(nxb);
            db.SaveChanges();
            return RedirectToAction("Index", "NhaXuatBan");
        }
        [HttpGet]
        public NHAXUATBAN GetNHAXUATBAN(int id)
        {
            return db.NHAXUATBANs.Where(nxb => nxb.MaNXB == id).SingleOrDefault();
           
           
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(GetNHAXUATBAN(id));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                var nxb = GetNHAXUATBAN(int.Parse(f["MaNXB"]));
                nxb.TenNXB = f["TenNXB"];
                nxb.DiaChi = f["DiaChi"];
                nxb.DienThoai = f["DienThoai"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");
            }    
        }

        [HttpGet]
        public ActionResult Xoa(int id)
        {
            return View(GetNHAXUATBAN(id));
        }

        [HttpPost]
        public ActionResult Xoa(NHAXUATBAN objnxb)
        { 
                var nxb = db.NHAXUATBANs.Where(n => n.MaNXB == objnxb.MaNXB).FirstOrDefault();
                db.NHAXUATBANs.Remove(nxb);
                db.SaveChanges();
                return RedirectToAction("Index");
           
        }

       
    }
}