using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using ADDLBankingApi.Models;

namespace ADDLBankingApi.Controllers
{
    [Authorize]
    public class ServicesController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Services
        public IQueryable<Service> GetService()
        {
            return db.Service;
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult GetService(int id)
        {
            Service service = db.Service.Find(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }

        // PUT: api/Services/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(service).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(service.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(service);
        
        }

        // POST: api/Services
        [ResponseType(typeof(Service))]
        public IHttpActionResult PostService(Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Service.Add(service);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = service.Id }, service);
        }

        // DELETE: api/Services/5
        [ResponseType(typeof(Service))]
        public IHttpActionResult DeleteService(int id)
        {
            try
            {
                Service service = db.Service.Find(id);
                if (service == null)
                {
                    return NotFound();
                }

                db.Service.Remove(service);
                db.SaveChanges();

                return Ok(service);


            }
            catch (Exception)
            {
                return Content(HttpStatusCode.NotAcceptable, "Database Service table relationship error.");
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

        private bool ServiceExists(int id)
        {
            return db.Service.Count(e => e.Id == id) > 0;
        }
    }
}