using Newtonsoft.Json.Linq;
using NguyenHoangNam.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Linq.Dynamic;

namespace NguyenHoangNam.Controllers
{
    public class HoangNamSearchController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: Search
        public ActionResult Search(string strSearch, int? page)
        {


            ViewBag.Search = strSearch;
            var chuDeList = db.CHUDEs.ToList();
            ViewBag.cd = chuDeList;
            if (!string.IsNullOrEmpty(strSearch))
            {
                var kq = db.SACHes.Where(s => s.TenSach.ToLower().Contains(strSearch.ToLower())).ToList();
                int pageSize = 6;
                int pageNumber = (page ?? 1);

                var sachList = kq.ToPagedList(pageNumber, pageSize);

                return View(sachList);

            }
            else
            {

                int pageSizes = 6;
                int pageNumbers = (page ?? 1);

                var sachList = db.SACHes.OrderBy(s => s.MaSach).ToPagedList(pageNumbers, pageSizes);

                return View(sachList);
            }



        }

        public ActionResult SearchTheoDanhMuc(string strSearch = null, int maCD = 0, int? page = 1)
        {
            ViewBag.Search = strSearch;



            var kq = db.SACHes.Select(b => b);



            kq = kq.Where(b => b.TenSach.Contains(strSearch));

            if (maCD != 0)
            {
                kq = kq.Where(b => b.CHUDE.MaCD == maCD);
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.cd = db.CHUDEs.ToList();

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var sortedResults = kq.OrderBy(b => b.TenSach);

            var sachListPaged = sortedResults.ToPagedList(pageNumber, pageSize);

            return View("Search", sachListPaged);

        }

        public ActionResult Group()
        {

            var kq = db.SACHes.GroupBy(s => s.MaCD);

            return View(kq);
        }

        public ActionResult ThongKe()
        {
            var kq = from s in db.SACHes
                     group s by s.MaCD into g
                     select new ReportInfo
                     {
                         Id = g.Key.ToString(),
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan),
                         Avg = Convert.ToDecimal(g.Average(n => n.SoLuongBan))
                     };
            return View(kq);
        }
        public ActionResult SearchPhanTrang(int? page, string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 5;
                int iPageNumber = (page ?? 1);
                var kq = (from s in db.SACHes where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s).ToList();
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();
        }


        public ActionResult SearchPhanTrangTuyChon(int? size, int? page, string strSearch = null)
        {

            List<SelectListItem> items = new List<SelectListItem>
    {
        new SelectListItem { Text = "3", Value = "3" },
        new SelectListItem { Text = "5", Value = "5" },
        new SelectListItem { Text = "10", Value = "10" },
        new SelectListItem { Text = "20", Value = "20" },
        new SelectListItem { Text = "25", Value = "25" },
        new SelectListItem { Text = "50", Value = "50" }
    };

            // Mark selected page size
            foreach (var item in items)
            {
                if (item.Value == size.ToString())
                {
                    item.Selected = true;
                }
            }

            ViewBag.size = items;
            ViewBag.currentSize = size;
            ViewBag.Search = strSearch;

            // Default values for page size and page number
            int iSize = size ?? 3;
            int iPageNumber = page ?? 1;

            // Perform search if a search term is provided
            if (!string.IsNullOrEmpty(strSearch))
            {
                var results = (from s in db.SACHes
                               where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch)
                               select s).ToList();

                // Use PagedList for pagination
                return View(results.ToPagedList(iPageNumber, iSize));
            }

            // Return an empty view if no search term is provided
            return View();
        }


        public ActionResult SearchPhanTrangSapXep(int? page, string sortProperty, string sortOrder = "", string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                if (sortOrder == "") ViewBag.SortOrder = "desc";
                if (sortOrder == "desc") ViewBag.SortOrder = "";
                if (sortOrder == "") ViewBag.SortOrder = "asc";
                if (String.IsNullOrEmpty(sortProperty))
                    sortProperty = "TenSach";
                ViewBag.SortProperty = sortProperty;

                var kq = from s in db.SACHes where s.TenSach.Contains(strSearch) ||
                         s.MoTa.Contains(strSearch) select s;
                if (sortOrder == "desc")
                {
                    kq = kq.OrderBy(sortProperty + " " + sortOrder);
                }
                else
                {
                    kq = kq.OrderBy(sortProperty); }
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();
        }
    }
}
    