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
    public class TimeDepositsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/TimeDeposits
        public IQueryable<TimeDeposit> GetTimeDeposit()
        {
            return db.TimeDeposit;
        }

        // GET: api/TimeDeposits/5
        [ResponseType(typeof(TimeDeposit))]
        public IHttpActionResult GetTimeDeposit(int id)
        {
            TimeDeposit timeDeposit = db.TimeDeposit.Find(id);
            if (timeDeposit == null)
            {
                return NotFound();
            }

            return Ok(timeDeposit);
        }

        // PUT: api/TimeDeposits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTimeDeposit(int id, TimeDeposit timeDeposit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timeDeposit.Id)
            {
                return BadRequest();
            }

            db.Entry(timeDeposit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeDepositExists(id))
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

        // POST: api/TimeDeposits
        [ResponseType(typeof(TimeDeposit))]
        public IHttpActionResult PostTimeDeposit(TimeDeposit timeDeposit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TimeDeposit.Add(timeDeposit);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = timeDeposit.Id }, timeDeposit);
        }

        // DELETE: api/TimeDeposits/5
        [ResponseType(typeof(TimeDeposit))]
        public IHttpActionResult DeleteTimeDeposit(int id)
        {
            TimeDeposit timeDeposit = db.TimeDeposit.Find(id);
            if (timeDeposit == null)
            {
                return NotFound();
            }

            db.TimeDeposit.Remove(timeDeposit);
            db.SaveChanges();

            return Ok(timeDeposit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimeDepositExists(int id)
        {
            return db.TimeDeposit.Count(e => e.Id == id) > 0;
        }
    }
}