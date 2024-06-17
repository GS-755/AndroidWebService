using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Customers
{
    public class UsersController : ApiController
    {
        // GET: api/Users
        [HttpGet]
        public async Task<List<NguoiDung>> Get()
        {
            List<NguoiDung> users = await DbInstance.Execute.GetDatabase.
                    NguoiDung.ToListAsync();

            return users;
        }
        // GET: api/Users/5
        [ResponseType(typeof(NguoiDung))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            NguoiDung nguoiDung = await DbInstance.Execute.GetDatabase.
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
        public async Task<IHttpActionResult> AddUser(NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DbInstance.Execute.GetDatabase.NguoiDung.Add(nguoiDung);

            try
            {
                await DbInstance.Execute.GetDatabase.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NguoiDungExists(nguoiDung.CCCD))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = nguoiDung.CCCD }, nguoiDung);
        }
        // PUT: api/Users/5
        [HttpPut]
        [ResponseType(typeof(NguoiDung))]
        public async Task<IHttpActionResult> EditUser(string id, NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != nguoiDung.CCCD.Trim())
            {
                return BadRequest();
            }

            try
            {
                DbInstance.Execute.GetDatabase.Entry(nguoiDung).State = EntityState.Modified;
                await DbInstance.Execute.GetDatabase.SaveChangesAsync();

                return Ok(nguoiDung);
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
                DbInstance.Execute.GetDatabase.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NguoiDungExists(string id)
        {
            return DbInstance.Execute.GetDatabase.NguoiDung.Count(e => e.CCCD == id) > 0;
        }
    }
}
