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
using LeaseManagement.Models;

namespace LeaseManagement.Controllers
{
    public class HouseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/HouseData/ListHouses
        [HttpGet]
        public IEnumerable<HouseDto> ListHouses()
        {
            List<House> Houses = db.Houses.ToList();
            List<HouseDto> HouseDtos = new List<HouseDto>();

            Houses.ForEach(a => HouseDtos.Add(new HouseDto()
            {
                HouseId = a.HouseId,
                HouseName = a.HouseName,
                HouseDescription = a.HouseDescription,
                OwnerName = a.Owner.OwnerName
            }));

            return HouseDtos;
        }

        // GET: api/HouseData/ListHousesForOwners/3
        [HttpGet]
        public IEnumerable<HouseDto> ListHousesForOwners(int id)
        {
            List<House> Houses = db.Houses.Where(a=>a.OwnerId==id).ToList();
            List<HouseDto> HouseDtos = new List<HouseDto>();

            Houses.ForEach(a => HouseDtos.Add(new HouseDto()
            {
                HouseId = a.HouseId,
                HouseName = a.HouseName,
                HouseDescription = a.HouseDescription,
                OwnerName = a.Owner.OwnerName
            }));

            return HouseDtos;
        }

        // GET: api/HouseData/ListHousesForTenant/3
        [HttpGet]
        public IEnumerable<HouseDto> ListHousesForTenant(int id)
        {
            List<House> Houses = db.Houses.Where(a => a.Tenants.Any(
                        k=>k.TenantId==id
                )).ToList();
            List<HouseDto> HouseDtos = new List<HouseDto>();

            Houses.ForEach(a => HouseDtos.Add(new HouseDto()
            {
                HouseId = a.HouseId,
                HouseName = a.HouseName,
                HouseDescription = a.HouseDescription,
                OwnerName = a.Owner.OwnerName
            }));

            return HouseDtos;
        }

        [HttpPost]
        [Route("api/House/AssociateHouseWithTenant/{houseid}/{tenantid}")]
        public IHttpActionResult AssociateHouseWithTenant(int houseid, int tenantid)
        {

            House SelectedHouse = db.Houses.Include(a => a.Tenants).Where(a => a.HouseId == houseid).FirstOrDefault();
            Tenant Tenant = db.Tenants.Find(tenantid);

            if (SelectedHouse == null || Tenant == null)
            {
                return NotFound();
            }


            SelectedHouse.Tenants.Add(Tenant);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("api/House/UnAssociateHouseWithTenant/{houseid}/{tenantid}")]
        public IHttpActionResult UnAssociateHouseWithTenant(int houseid, int tenantid)
        {

            House SelectedHouse = db.Houses.Include(a => a.Tenants).Where(a => a.HouseId == houseid).FirstOrDefault();
            Tenant Tenant = db.Tenants.Find(tenantid);

            if (SelectedHouse == null || Tenant == null)
            {
                return NotFound();
            }


            SelectedHouse.Tenants.Remove(Tenant);
            db.SaveChanges();

            return Ok();
        }


        // GET: api/HouseData/FindHouse/5
        [ResponseType(typeof(House))]
        [HttpGet]
        public IHttpActionResult FindHouse(int id)
        {
            House House = db.Houses.Find(id);
            HouseDto HouseDtos = new HouseDto()
            {
                HouseId = House.HouseId,
                HouseName = House.HouseName,
                HouseDescription = House.HouseDescription,
                OwnerName = House.Owner.OwnerName
            };
            if (House == null)
            {
                return NotFound();
            }

            return Ok(HouseDtos);
        }

        // POST: api/HouseData/UpdateHouse/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateHouse(int id, House house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != house.HouseId)
            {
                return BadRequest();
            }

            db.Entry(house).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HouseExists(id))
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

        // POST: api/HouseData/AddHouse
        [ResponseType(typeof(House))]
        [HttpPost]
        public IHttpActionResult AddHouse(House house)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Houses.Add(house);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = house.HouseId }, house);
        }

        // POST: api/HouseData/DeleteHouse/5
        [ResponseType(typeof(House))]
        [HttpPost]
        public IHttpActionResult DeleteHouse(int id)
        {
            House house = db.Houses.Find(id);
            if (house == null)
            {
                return NotFound();
            }

            db.Houses.Remove(house);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HouseExists(int id)
        {
            return db.Houses.Count(e => e.HouseId == id) > 0;
        }
    }
}