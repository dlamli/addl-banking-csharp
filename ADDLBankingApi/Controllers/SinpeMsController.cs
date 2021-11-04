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
    public class SinpeMsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/SinpeMs
        public IQueryable<SinpeM> GetSinpeM()
        {
            return db.SinpeM;
        }

        // GET: api/SinpeMs/5
        [ResponseType(typeof(SinpeM))]
        public IHttpActionResult GetSinpeM(int id)
        {
            SinpeM sinpeM = db.SinpeM.Find(id);
            if (sinpeM == null)
            {
                return NotFound();
            }

            return Ok(sinpeM);
        }

        // PUT: api/SinpeMs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSinpeM(int id, SinpeM sinpeM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sinpeM.Id)
            {
                return BadRequest();
            }

            db.Entry(sinpeM).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SinpeMExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SinpeMs
        [ResponseType(typeof(SinpeM))]
        public IHttpActionResult PostSinpeM(SinpeM sinpeM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SinpeM.Add(sinpeM);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sinpeM.Id }, sinpeM);
        }

        // DELETE: api/SinpeMs/5
        [ResponseType(typeof(SinpeM))]
        public IHttpActionResult DeleteSinpeM(int id)
        {
            SinpeM sinpeM = db.SinpeM.Find(id);
            if (sinpeM == null)
            {
                return NotFound();
            }

            db.SinpeM.Remove(sinpeM);
            db.SaveChanges();

            return Ok(sinpeM);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SinpeMExists(int id)
        {
            return db.SinpeM.Count(e => e.Id == id) > 0;
        }
    }
}