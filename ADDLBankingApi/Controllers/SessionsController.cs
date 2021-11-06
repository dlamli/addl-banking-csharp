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
    public class SessionsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Sessions
        public IQueryable<Session> GetSession()
        {
            return db.Session;
        }

        // GET: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult GetSession(int id)
        {
            Session session = db.Session.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        // PUT: api/Sessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSession(Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(session).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(session.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(session);
        }

        // POST: api/Sessions
        [ResponseType(typeof(Session))]
        public IHttpActionResult PostSession(Session session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Session.Add(session);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = session.Id }, session);
        }

        // DELETE: api/Sessions/5
        [ResponseType(typeof(Session))]
        public IHttpActionResult DeleteSession(int id)
        {
            Session session = db.Session.Find(id);
            if (session == null)
            {
                return NotFound();
            }

            db.Session.Remove(session);
            db.SaveChanges();

            return Ok(session);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SessionExists(int id)
        {
            return db.Session.Count(e => e.Id == id) > 0;
        }
    }
}