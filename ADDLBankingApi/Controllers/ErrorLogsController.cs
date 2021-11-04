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
    public class ErrorLogsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/ErrorLogs
        public IQueryable<ErrorLog> GetErrorLog()
        {
            return db.ErrorLog;
        }

        // GET: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult GetErrorLog(int id)
        {
            ErrorLog errorLog = db.ErrorLog.Find(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            return Ok(errorLog);
        }

        // PUT: api/ErrorLogs/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutErrorLog(int id, ErrorLog errorLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != errorLog.Id)
            {
                return BadRequest();
            }

            db.Entry(errorLog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorLogExists(id))
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

        // POST: api/ErrorLogs
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult PostErrorLog(ErrorLog errorLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ErrorLog.Add(errorLog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = errorLog.Id }, errorLog);
        }

        // DELETE: api/ErrorLogs/5
        [ResponseType(typeof(ErrorLog))]
        public IHttpActionResult DeleteErrorLog(int id)
        {
            ErrorLog errorLog = db.ErrorLog.Find(id);
            if (errorLog == null)
            {
                return NotFound();
            }

            db.ErrorLog.Remove(errorLog);
            db.SaveChanges();

            return Ok(errorLog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ErrorLogExists(int id)
        {
            return db.ErrorLog.Count(e => e.Id == id) > 0;
        }
    }
}