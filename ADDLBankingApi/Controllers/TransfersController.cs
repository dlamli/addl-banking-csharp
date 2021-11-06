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
    public class TransfersController : ApiController
    {
        private ADDL_Entities db = new ADDL_Entities();

        // GET: api/Transfers
        public IQueryable<Transfer> GetTransfer()
        {
            return db.Transfer;
        }

        // GET: api/Transfers/5
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult GetTransfer(int id)
        {
            Transfer transfer = db.Transfer.Find(id);
            if (transfer == null)
            {
                return NotFound();
            }

            return Ok(transfer);
        }

        // PUT: api/Transfers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransfer(Transfer transfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(transfer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferExists(transfer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(transfer);
        }

        // POST: api/Transfers
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult PostTransfer(Transfer transfer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transfer.Add(transfer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transfer.Id }, transfer);
        }

        // DELETE: api/Transfers/5
        [ResponseType(typeof(Transfer))]
        public IHttpActionResult DeleteTransfer(int id)
        {
            Transfer transfer = db.Transfer.Find(id);
            if (transfer == null)
            {
                return NotFound();
            }

            db.Transfer.Remove(transfer);
            db.SaveChanges();

            return Ok(transfer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransferExists(int id)
        {
            return db.Transfer.Count(e => e.Id == id) > 0;
        }
    }
}