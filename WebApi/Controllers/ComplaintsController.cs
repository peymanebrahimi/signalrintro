using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Hubs;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class ComplaintsController : ApiControllerWithHub<MyHub>
    {
        private CustomerContext db = new CustomerContext();

        public IQueryable<Complaints> GetCustomerComplaints()
        {
            return db.CustomerComplaints;
        }

        [ResponseType(typeof(Complaints))]
        public async Task<IHttpActionResult> GetCustomerComplaints(int id)
        {
            var customerComplaints = await db.CustomerComplaints.Where(x => x.CustomerId == id.ToString()).ToListAsync();
            if (customerComplaints == null)
            {
                return NotFound();
            }
            return Ok(customerComplaints);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCustomerComplaints(int id, Complaints complaints)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != complaints.Id)
            {
                return BadRequest();
            }

            db.Entry(complaints).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                Hub.Clients.Group(complaints.CustomerId).updateItem(complaints);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerComplaintsExists(id))
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

        [ResponseType(typeof(Complaints))]
        public async Task<IHttpActionResult> PostCustomerComplaints(Complaints complaints)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CustomerComplaints.Add(complaints);
            await db.SaveChangesAsync();
            Hub.Clients.Group(complaints.CustomerId).addItem(complaints);
            return CreatedAtRoute("DefaultApi", new { id = complaints.Id }, complaints);
        }

        [ResponseType(typeof(Complaints))]
        public async Task<IHttpActionResult> DeleteCustomerComplaints(int id)
        {
            Complaints complaints = await db.CustomerComplaints.FindAsync(id);
            if (complaints == null)
            {
                return NotFound();
            }

            db.CustomerComplaints.Remove(complaints);
            await db.SaveChangesAsync();
            Hub.Clients.Group(complaints.CustomerId).deleteItem(complaints);
            return Ok(complaints);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerComplaintsExists(int id)
        {
            return db.CustomerComplaints.Count(e => e.Id == id) > 0;
        }
    }
}