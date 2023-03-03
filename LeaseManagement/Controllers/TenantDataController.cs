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
using System.Web.Mvc;
using System.Web.Routing;
using LeaseManagement.Models;

namespace LeaseManagement.Controllers
{
    public class TenantDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/TenantData/ListTenants
        [System.Web.Http.HttpGet]
        public IEnumerable<TenantDto> ListTenants()
        {
            List<Tenant> Tenant = db.Tenants.ToList();
            List<TenantDto> TenantDtos = new List<TenantDto>();

            Tenant.ForEach(a => TenantDtos.Add(new TenantDto()
            {
                TenantId = a.TenantId,
                TenantName = a.TenantName,
                TenantDescription = a.TenantDescription
            }));

            return TenantDtos;
        }

        // GET: api/TenantData/ListTenantsForHouse/2
        [System.Web.Http.HttpGet]
        public IEnumerable<TenantDto> ListTenantsForHouse(int id)
        {
            List<Tenant> Tenant = db.Tenants.Where(
                    k=>k.House.Any(
                        a=>a.HouseId == id)
                    ).ToList();
            List<TenantDto> TenantDtos = new List<TenantDto>();

            Tenant.ForEach(a => TenantDtos.Add(new TenantDto()
            {
                TenantId = a.TenantId,
                TenantName = a.TenantName,
                TenantDescription = a.TenantDescription
            }));

            return TenantDtos;
        }

        // GET: api/TenantData/ListOtherTenants/2
        [System.Web.Http.HttpGet]
        public IEnumerable<TenantDto> ListOtherTenants(int id)
        {
            List<Tenant> Tenant = db.Tenants.Where(
                    k => !k.House.Any(
                        a => a.HouseId == id)
                    ).ToList();
            List<TenantDto> TenantDtos = new List<TenantDto>();

            Tenant.ForEach(a => TenantDtos.Add(new TenantDto()
            {
                TenantId = a.TenantId,
                TenantName = a.TenantName,
                TenantDescription = a.TenantDescription
            }));

            return TenantDtos;
        }

        // GET: api/TenantData/FindTenant/5
        [ResponseType(typeof(Tenant))]
        [System.Web.Http.HttpGet]
        public IHttpActionResult FindTenant(int id)
        {
            Tenant Tenant = db.Tenants.Find(id);
            TenantDto TenantDtos = new TenantDto()
            {
                TenantId = Tenant.TenantId,
                TenantName = Tenant.TenantName,
                TenantDescription = Tenant.TenantDescription
            };
            if (Tenant == null)
            {
                return NotFound();
            }

            return Ok(TenantDtos);
        }

        // PUT: api/TenantData/UpdateTenant/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTenant(int id, Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenant.TenantId)
            {
                return BadRequest();
            }

            db.Entry(tenant).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
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

        // POST: api/TenantData/AddTenant
        [ResponseType(typeof(Tenant))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult AddTenant(Tenant tenant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tenants.Add(tenant);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tenant.TenantId }, tenant);
        }

        // DELETE: api/TenantData/DeleteTenant/5
        [ResponseType(typeof(Tenant))]
        [System.Web.Http.HttpPost]
        public IHttpActionResult DeleteTenant(int id)
        {
            Tenant tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return NotFound();
            }

            db.Tenants.Remove(tenant);
            db.SaveChanges();

            return Ok(tenant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TenantExists(int id)
        {
            return db.Tenants.Count(e => e.TenantId == id) > 0;
        }
    }
}