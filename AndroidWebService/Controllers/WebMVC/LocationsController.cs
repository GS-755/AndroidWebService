using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebMVC
{
    public class LocationsController : Controller
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: Locations
        public ActionResult Index()
        {
            return View(db.ViTri.ToList());
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTri viTri = db.ViTri.Find(id);
            if (viTri == null)
            {
                return HttpNotFound();
            }
            return View(viTri);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaVT,Quan,KinhDo,ViDo")] ViTri viTri)
        {
            if (ModelState.IsValid)
            {
                db.ViTri.Add(viTri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viTri);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTri viTri = db.ViTri.Find(id);
            if (viTri == null)
            {
                return HttpNotFound();
            }
            return View(viTri);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaVT,Quan,KinhDo,ViDo")] ViTri viTri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viTri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viTri);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViTri viTri = db.ViTri.Find(id);
            if (viTri == null)
            {
                return HttpNotFound();
            }
            return View(viTri);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViTri viTri = db.ViTri.Find(id);
            db.ViTri.Remove(viTri);
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
