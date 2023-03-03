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
using System.Web.Routing;
using LeaseManagement.Models;

namespace LeaseManagement.Controllers
{
    public class OwnerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/OwnerData/ListOwners
        [HttpGet]
        public IEnumerable<OwnerDto> ListOwners()
        {
            List<Owner> Owner = db.Owners.ToList();
            List<OwnerDto> OwnerDtos = new List<OwnerDto>();

            Owner.ForEach(a => OwnerDtos.Add(new OwnerDto()
            {
                OwnerId = a.OwnerId,
                OwnerName = a.OwnerName,
                OwnerDescription = a.OwnerDescription
            }));

            return OwnerDtos;
        }

        // GET: api/OwnerData/FindOwner/5
        [ResponseType(typeof(Owner))]
        [HttpGet]
        public IHttpActionResult FindOwner(int id)
        {
            Owner Owner = db.Owners.Find(id);
            OwnerDto OwnerDtos = new OwnerDto()
            {
                OwnerId = Owner.OwnerId,
                OwnerName = Owner.OwnerName,
                OwnerDescription = Owner.OwnerDescription
            };
            if (Owner == null)
            {
                return NotFound();
            }

            return Ok(OwnerDtos);
        }

        // PUT: api/OwnerData/UpdateOwner/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateOwner(int id, Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != owner.OwnerId)
            {
                return BadRequest();
            }

            db.Entry(owner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(id))
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

        // POST: api/OwnerData/AddOwner
        [ResponseType(typeof(Owner))]
        [HttpPost]
        public IHttpActionResult AddOwner(Owner owner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Owners.Add(owner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = owner.OwnerId }, owner);
        }

        // DELETE: api/OwnerData/DeleteOwner/5
        [ResponseType(typeof(Owner))]
        [HttpPost]
        public IHttpActionResult DeleteOwner(int id)
        {
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }

            db.Owners.Remove(owner);
            db.SaveChanges();

            return Ok(owner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OwnerExists(int id)
        {
            return db.Owners.Count(e => e.OwnerId == id) > 0;
        }
    }
}