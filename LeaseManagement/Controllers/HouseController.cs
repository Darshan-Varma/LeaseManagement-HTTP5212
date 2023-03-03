using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LeaseManagement.Models.ViewModels;
using LeaseManagement.Models;
using System.Diagnostics;

namespace LeaseManagement.Controllers
{
    public class HouseController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static HouseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44322/api/");
        }
        // GET: House/List
        public ActionResult List()
        {
            string url = "HouseData/ListHouses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<HouseDto> house = response.Content.ReadAsAsync<IEnumerable<HouseDto>>().Result;
           

            return View(house);
        }

        // GET: House/Details/5
        public ActionResult Details(int id)
        {
            HouseDetails ViewModel = new HouseDetails();
            string url = "HouseData/FindHouse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HouseDto house = response.Content.ReadAsAsync<HouseDto>().Result;
            ViewModel.House = house;

            url = "tenantdata/listtenantsforhouse/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TenantDto> Tenants = response.Content.ReadAsAsync<IEnumerable<TenantDto>>().Result;
            ViewModel.Tenant = Tenants;

            url = "tenantdata/ListOtherTenants/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TenantDto> OtherTenants = response.Content.ReadAsAsync<IEnumerable<TenantDto>>().Result;
            ViewModel.OtherTenants = OtherTenants;

            return View(ViewModel);
        }

        //POST: House/Associate/{houseid}
        [HttpPost]
        public ActionResult Associate(int id, int tenantId)
        {
            string url = "House/AssociateHouseWithTenant/" + id + "/" + tenantId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        //POST: House/UnAssociate/{houseid}
        [HttpGet]
        public ActionResult UnAssociate(int id, int tenantId)
        {
            string url = "House/UnAssociateHouseWithTenant/" + id + "/" + tenantId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: House/New
        public ActionResult New()
        {
            string url = "OwnerData/ListOwners";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<OwnerDto> owner = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;


            return View(owner);
        }

        // POST: House/Create
        [HttpPost]
        public ActionResult Create(House House)
        {
            //curl -H "Content-Type:application/json" -d @house.json https://localhost:44322/api/HouseData/addhouse 
            string url = "HouseData/addhouse";

            string jsonpayload = jss.Serialize(House);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: House/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateHouse ViewModel = new UpdateHouse();
            string url = "HouseData/findhouse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HouseDto house = response.Content.ReadAsAsync<HouseDto>().Result;

            ViewModel.House = house;

            url = "OwnerData/ListOwners/";
            response = client.GetAsync(url).Result;
            IEnumerable<OwnerDto> owner = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;

            ViewModel.Owner = owner;

            return View(ViewModel);
        }

        // POST: House/Update/5
        [HttpPost]
        public ActionResult Update(int id, House House)
        {
            string url = "HouseData/updatehouse/" + id;
            string jsonpayload = jss.Serialize(House);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: House/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "HouseData/findhouse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HouseDto house = response.Content.ReadAsAsync<HouseDto>().Result;


            return View(house);
        }

        // POST: House/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, House House)
        {
            string url = "HouseData/deletehouse/" + id;
            string jsonpayload = jss.Serialize(House);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
