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
    public class CardRequestsController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/CardRequests
        public IQueryable<CardRequest> GetCardRequest()
        {
            return db.CardRequest;
        }

        // GET: api/CardRequests/5
        [ResponseType(typeof(CardRequest))]
        public IHttpActionResult GetCardRequest(int id)
        {
            CardRequest cardRequest = db.CardRequest.Find(id);
            if (cardRequest == null)
            {
                return NotFound();
            }

            return Ok(cardRequest);
        }

        // PUT: api/CardRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCardRequest( CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.Entry(cardRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardRequestExists(cardRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(cardRequest);
        }

        // POST: api/CardRequests
        [ResponseType(typeof(CardRequest))]
        public IHttpActionResult PostCardRequest(CardRequest cardRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CardRequest.Add(cardRequest);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cardRequest.Id }, cardRequest);
        }

        // DELETE: api/CardRequests/5
        [ResponseType(typeof(CardRequest))]
        public IHttpActionResult DeleteCardRequest(int id)
        {
            CardRequest cardRequest = db.CardRequest.Find(id);
            if (cardRequest == null)
            {
                return NotFound();
            }

            db.CardRequest.Remove(cardRequest);
            db.SaveChanges();

            return Ok(cardRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardRequestExists(int id)
        {
            return db.CardRequest.Count(e => e.Id == id) > 0;
        }
    }
}