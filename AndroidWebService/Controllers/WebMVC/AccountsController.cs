using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AndroidWebService.Models;
using AndroidWebService.Models.Utils;

namespace AndroidWebService.Controllers.WebMVC
{
    public class AccountsController : Controller
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: Accounts
        public ActionResult Index()
        {
            var taiKhoan = db.TaiKhoan.Include(t => t.NguoiDung).Include(t => t.VaiTro);
            return View(taiKhoan.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.CCCD = new SelectList(db.NguoiDung, "CCCD", "CCCD");
            ViewBag.MaVaiTro = new SelectList(db.VaiTro, "MaVaiTro", "TenVaiTro");

            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenDangNhap,MatKhau,Email,SoDT,MaVaiTro,CCCD")] TaiKhoan taiKhoan)
        {
            string authTmp = SHA256.Get(taiKhoan.MatKhau);
            taiKhoan.MatKhau = authTmp;
            if (ModelState.IsValid)
            {
                db.TaiKhoan.Add(taiKhoan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CCCD = new SelectList(db.NguoiDung, "CCCD", "Ho", taiKhoan.CCCD);
            ViewBag.MaVaiTro = new SelectList(db.VaiTro, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);

            return View(taiKhoan);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CCCD = new SelectList(db.NguoiDung, "CCCD", "Ho", taiKhoan.CCCD);
            ViewBag.MaVaiTro = new SelectList(db.VaiTro, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenDangNhap,MatKhau,Email,SoDT,MaVaiTro,CCCD")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taiKhoan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CCCD = new SelectList(db.NguoiDung, "CCCD", "Ho", taiKhoan.CCCD);
            ViewBag.MaVaiTro = new SelectList(db.VaiTro, "MaVaiTro", "TenVaiTro", taiKhoan.MaVaiTro);
            return View(taiKhoan);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
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
