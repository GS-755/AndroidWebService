using System.IO;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebMVC
{
    public class MotelsController : Controller
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        public string GetImagePath(string fileName)
        {
            return Path.Combine(Server.MapPath(PhongTro.SERVER_IMG_PATH), fileName);
        }
        // GET: Motels
        public ActionResult Index()
        {
            var phongTro = db.PhongTro.Include(p => p.TaiKhoan).Include(p => p.TrangThai).Include(p => p.ViTri);
            return View(phongTro.ToList());
        }

        // GET: Motels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongTro phongTro = db.PhongTro.Find(id);
            if (phongTro == null)
            {
                return HttpNotFound();
            }
            return View(phongTro);
        }

        // GET: Motels/Create
        public ActionResult Create()
        {
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "TenDangNhap");
            ViewBag.MaTT = new SelectList(db.TrangThai, "MaTT", "TenTT");
            ViewBag.MaVT = new SelectList(db.ViTri, "MaVT", "Quan");
            return View();
        }

        // POST: Motels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PhongTro phongTro)
        {
            if (ModelState.IsValid)
            {
                if (phongTro.UploadImage != null)
                {
                    string copyName = $"{phongTro.TenDangNhap.Trim()}_{phongTro.NgayDang.ToString("mmddyyyy_HHmm")}";
                    string extension = Path.GetExtension(phongTro.UploadImage.FileName);
                    copyName += extension;
                    phongTro.HinhAnh = copyName;
                    phongTro.UploadImage.SaveAs(GetImagePath(copyName));
                }
                db.PhongTro.Add(phongTro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", phongTro.TenDangNhap);
            ViewBag.MaTT = new SelectList(db.TrangThai, "MaTT", "TenTT", phongTro.MaTT);
            ViewBag.MaVT = new SelectList(db.ViTri, "MaVT", "Quan", phongTro.MaVT);
            return View(phongTro);
        }

        // GET: Motels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongTro phongTro = db.PhongTro.Find(id);
            if (phongTro == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", phongTro.TenDangNhap);
            ViewBag.MaTT = new SelectList(db.TrangThai, "MaTT", "TenTT", phongTro.MaTT);
            ViewBag.MaVT = new SelectList(db.ViTri, "MaVT", "Quan", phongTro.MaVT);
            return View(phongTro);
        }

        // POST: Motels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PhongTro phongTro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phongTro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TenDangNhap = new SelectList(db.TaiKhoan, "TenDangNhap", "MatKhau", phongTro.TenDangNhap);
            ViewBag.MaTT = new SelectList(db.TrangThai, "MaTT", "TenTT", phongTro.MaTT);
            ViewBag.MaVT = new SelectList(db.ViTri, "MaVT", "Quan", phongTro.MaVT);
            return View(phongTro);
        }

        // GET: Motels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhongTro phongTro = db.PhongTro.Find(id);
            if (phongTro == null)
            {
                return HttpNotFound();
            }
            return View(phongTro);
        }

        // POST: Motels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhongTro phongTro = db.PhongTro.Find(id);
            db.PhongTro.Remove(phongTro);
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
