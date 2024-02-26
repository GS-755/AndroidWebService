using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebMVC
{
    public class OrderRoomDetailsController : Controller
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: OrderRoomDetails
        public ActionResult Index()
        {
            var cTDatPhong = db.CTDatPhong.Include(c => c.TaiKhoan).Include(c => c.PhongTro);
            return View(cTDatPhong.ToList());
        }

        // GET: OrderRoomDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDatPhong cTDatPhong = db.CTDatPhong.Find(id);
            if (cTDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(cTDatPhong);
        }

        // GET: OrderRoomDetails/Create
        public ActionResult Create()
        {
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau");
            ViewBag.MaPT = new SelectList(db.PhongTro, "MaPT", "TieuDe");
            return View();
        }

        // POST: OrderRoomDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NgayDat,GhiChu,TienCoc,MaPT,TenDangNhap")] CTDatPhong cTDatPhong)
        {
            if (ModelState.IsValid)
            {
                db.CTDatPhong.Add(cTDatPhong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", cTDatPhong.TenDangNhap);
            ViewBag.MaPT = new SelectList(db.PhongTro, "MaPT", "TieuDe", cTDatPhong.MaPT);
            return View(cTDatPhong);
        }

        // GET: OrderRoomDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDatPhong cTDatPhong = db.CTDatPhong.Find(id);
            if (cTDatPhong == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", cTDatPhong.TenDangNhap);
            ViewBag.MaPT = new SelectList(db.PhongTro, "MaPT", "TieuDe", cTDatPhong.MaPT);
            return View(cTDatPhong);
        }

        // POST: OrderRoomDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NgayDat,GhiChu,TienCoc,MaPT,TenDangNhap")] CTDatPhong cTDatPhong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTDatPhong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", cTDatPhong.TenDangNhap);
            ViewBag.MaPT = new SelectList(db.PhongTro, "MaPT", "TieuDe", cTDatPhong.MaPT);
            return View(cTDatPhong);
        }

        // GET: OrderRoomDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDatPhong cTDatPhong = db.CTDatPhong.Find(id);
            if (cTDatPhong == null)
            {
                return HttpNotFound();
            }
            return View(cTDatPhong);
        }

        // POST: OrderRoomDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CTDatPhong cTDatPhong = db.CTDatPhong.Find(id);
            db.CTDatPhong.Remove(cTDatPhong);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
