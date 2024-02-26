using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebMVC
{
    public class RolesController : Controller
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            return View(await db.VaiTro.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = await db.VaiTro.FindAsync(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MaVaiTro,TenVaiTro")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                db.VaiTro.Add(vaiTro);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vaiTro);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = await db.VaiTro.FindAsync(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MaVaiTro,TenVaiTro")] VaiTro vaiTro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaiTro).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vaiTro);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VaiTro vaiTro = await db.VaiTro.FindAsync(id);
            if (vaiTro == null)
            {
                return HttpNotFound();
            }
            return View(vaiTro);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VaiTro vaiTro = await db.VaiTro.FindAsync(id);
            db.VaiTro.Remove(vaiTro);
            await db.SaveChangesAsync();
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
