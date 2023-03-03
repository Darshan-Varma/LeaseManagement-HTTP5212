using LeaseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LeaseManagement.Models.ViewModels;
using System.Diagnostics;

namespace LeaseManagement.Controllers
{
    public class TenantController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static TenantController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44322/api/");
        }
        // GET: Tenant/List
        public ActionResult List()
        {
            string url = "TenantData/ListTenants";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<TenantDto> tenant = response.Content.ReadAsAsync<IEnumerable<TenantDto>>().Result;


            return View(tenant);
        }

        // GET: Tenant/Details/5
        public ActionResult Details(int id)
        {
            TenantDetails ViewModel = new TenantDetails();

            string url = "TenantData/FindTenant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            TenantDto tenant = response.Content.ReadAsAsync<TenantDto>().Result;
            ViewModel.Tenant = tenant;

            url = "housedata/ListHousesForTenant/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<HouseDto> LeasedHousesByTenant = response.Content.ReadAsAsync<IEnumerable<HouseDto>>().Result;
            ViewModel.LeasedHouses = LeasedHousesByTenant;

            return View(ViewModel);
        }

        // GET: Tenant/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Tenant/Create
        [HttpPost]
        public ActionResult Create(Tenant Tenant)
        {
            string url = "TenantData/addTenant";

            string jsonpayload = jss.Serialize(Tenant);

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

        // GET: Tenant/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Tenantdata/findTenant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TenantDto Tenant = response.Content.ReadAsAsync<TenantDto>().Result;
            return View(Tenant);
        }

        // POST: Tenant/Update/5
        [HttpPost]
        public ActionResult Update(int id, Tenant Tenant)
        {
            string url = "Tenantdata/updatetenant/" + id;
            string jsonpayload = jss.Serialize(Tenant);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Tenant/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TenantData/findTenant/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TenantDto Tenant = response.Content.ReadAsAsync<TenantDto>().Result;


            return View(Tenant);
        }

        // POST: Tenant/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Tenant Tenant)
        {
            string url = "TenantData/deleteTenant/" + id;
            string jsonpayload = jss.Serialize(Tenant);

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
