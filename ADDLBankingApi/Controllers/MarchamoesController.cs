using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ADDLBankingApi.Models;

namespace ADDLBankingApi.Controllers
{
    [Authorize]
    public class MarchamoesController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Marchamoes
        public IQueryable<Marchamo> GetMarchamo()
        {
            return db.Marchamo;
        }

        // GET: api/Marchamoes/5
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult GetMarchamo(int id)
        {
            Marchamo marchamo = db.Marchamo.Find(id);
            if (marchamo == null)
            {
                return NotFound();
            }

            return Ok(marchamo);
        }

        // PUT: api/Marchamoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMarchamo(Marchamo marchamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(marchamo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarchamoExists(marchamo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(marchamo);
        }

        // POST: api/Marchamoes
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult PostMarchamo(Marchamo marchamo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Marchamo.Add(marchamo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = marchamo.Id }, marchamo);
        }

        // DELETE: api/Marchamoes/5
        [ResponseType(typeof(Marchamo))]
        public IHttpActionResult DeleteMarchamo(int id)
        {
            Marchamo marchamo = db.Marchamo.Find(id);
            if (marchamo == null)
            {
                return NotFound();
            }

            db.Marchamo.Remove(marchamo);
            db.SaveChanges();

            return Ok(marchamo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MarchamoExists(int id)
        {
            return db.Marchamo.Count(e => e.Id == id) > 0;
        }
    }
}