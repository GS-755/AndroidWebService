using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Customers
{
    public class UsersController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Users
        [HttpGet]
        public async Task<List<NguoiDung>> Get()
        {
            List<NguoiDung> users = await db.
                    NguoiDung.ToListAsync();

            return users;
        }
        // GET: api/Users/5
        [ResponseType(typeof(NguoiDung))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            NguoiDung nguoiDung = await db.
                    NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return Ok(nguoiDung);
        }

        // POST: api/Users
        [ResponseType(typeof(NguoiDung))]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(NguoiDung user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NguoiDung.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NguoiDungExists(user.CCCD))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = user.CCCD }, user);
        }
        // PUT: api/Users/5
        [HttpPut]
        [ResponseType(typeof(NguoiDung))]
        public async Task<IHttpActionResult> EditUser(string id, NguoiDung user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != user.CCCD.Trim())
            {
                return BadRequest();
            }

            try
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Ok(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguoiDungExists(id.Trim()))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NguoiDungExists(string id)
        {
            return db.NguoiDung.Count(e => e.CCCD == id) > 0;
        }
    }
}
