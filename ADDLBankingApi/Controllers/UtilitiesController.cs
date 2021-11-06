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
    public class UtilitiesController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Utilities
        public IQueryable<Utility> GetUtility()
        {
            return db.Utility;
        }

        // GET: api/Utilities/5
        [ResponseType(typeof(Utility))]
        public IHttpActionResult GetUtility(int id)
        {
            Utility utility = db.Utility.Find(id);
            if (utility == null)
            {
                return NotFound();
            }

            return Ok(utility);
        }

        // PUT: api/Utilities/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUtility(Utility utility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(utility).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilityExists(utility.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(utility);
        }

        // POST: api/Utilities
        [ResponseType(typeof(Utility))]
        public IHttpActionResult PostUtility(Utility utility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Utility.Add(utility);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = utility.Id }, utility);
        }

        // DELETE: api/Utilities/5
        [ResponseType(typeof(Utility))]
        public IHttpActionResult DeleteUtility(int id)
        {
            Utility utility = db.Utility.Find(id);
            if (utility == null)
            {
                return NotFound();
            }

            db.Utility.Remove(utility);
            db.SaveChanges();

            return Ok(utility);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UtilityExists(int id)
        {
            return db.Utility.Count(e => e.Id == id) > 0;
        }
    }
}