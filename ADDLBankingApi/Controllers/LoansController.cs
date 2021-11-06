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
    public class LoansController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Loans
        public IQueryable<Loan> GetLoan()
        {
            return db.Loan;
        }

        // GET: api/Loans/5
        [ResponseType(typeof(Loan))]
        public IHttpActionResult GetLoan(int id)
        {
            Loan loan = db.Loan.Find(id);
            if (loan == null)
            {
                return NotFound();
            }

            return Ok(loan);
        }

        // PUT: api/Loans/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLoan(Loan loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(loan).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loan.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(loan);
        }

        // POST: api/Loans
        [ResponseType(typeof(Loan))]
        public IHttpActionResult PostLoan(Loan loan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Loan.Add(loan);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = loan.Id }, loan);
        }

        // DELETE: api/Loans/5
        [ResponseType(typeof(Loan))]
        public IHttpActionResult DeleteLoan(int id)
        {
            Loan loan = db.Loan.Find(id);
            if (loan == null)
            {
                return NotFound();
            }

            db.Loan.Remove(loan);
            db.SaveChanges();

            return Ok(loan);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LoanExists(int id)
        {
            return db.Loan.Count(e => e.Id == id) > 0;
        }
    }
}