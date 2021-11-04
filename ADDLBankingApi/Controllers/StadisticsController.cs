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
    public class StadisticsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Stadistics
        public IQueryable<Stadistic> GetStadistic()
        {
            return db.Stadistic;
        }

        // GET: api/Stadistics/5
        [ResponseType(typeof(Stadistic))]
        public IHttpActionResult GetStadistic(int id)
        {
            Stadistic stadistic = db.Stadistic.Find(id);
            if (stadistic == null)
            {
                return NotFound();
            }

            return Ok(stadistic);
        }

        // PUT: api/Stadistics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStadistic(int id, Stadistic stadistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stadistic.Id)
            {
                return BadRequest();
            }

            db.Entry(stadistic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StadisticExists(id))
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

        // POST: api/Stadistics
        [ResponseType(typeof(Stadistic))]
        public IHttpActionResult PostStadistic(Stadistic stadistic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stadistic.Add(stadistic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = stadistic.Id }, stadistic);
        }

        // DELETE: api/Stadistics/5
        [ResponseType(typeof(Stadistic))]
        public IHttpActionResult DeleteStadistic(int id)
        {
            Stadistic stadistic = db.Stadistic.Find(id);
            if (stadistic == null)
            {
                return NotFound();
            }

            db.Stadistic.Remove(stadistic);
            db.SaveChanges();

            return Ok(stadistic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StadisticExists(int id)
        {
            return db.Stadistic.Count(e => e.Id == id) > 0;
        }
    }
}