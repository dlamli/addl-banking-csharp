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
    public class CurrenciesController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Currencies
        public IQueryable<Currency> GetCurrency()
        {
            return db.Currency;
        }

        // GET: api/Currencies/5
        [ResponseType(typeof(Currency))]
        public IHttpActionResult GetCurrency(int id)
        {
            Currency currency = db.Currency.Find(id);
            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }

        // PUT: api/Currencies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCurrency( Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(currency).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(currency.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(currency);
        }

        // POST: api/Currencies
        [ResponseType(typeof(Currency))]
        public IHttpActionResult PostCurrency(Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Currency.Add(currency);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = currency.Id }, currency);
        }

        // DELETE: api/Currencies/5
        [ResponseType(typeof(Currency))]
        public IHttpActionResult DeleteCurrency(int id)
        {
            Currency currency = db.Currency.Find(id);
            if (currency == null)
            {
                return NotFound();
            }

            db.Currency.Remove(currency);
            db.SaveChanges();

            return Ok(currency);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CurrencyExists(int id)
        {
            return db.Currency.Count(e => e.Id == id) > 0;
        }
    }
}