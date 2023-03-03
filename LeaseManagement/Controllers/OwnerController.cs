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
    public class OwnerController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();
        static OwnerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44322/api/");
        }
        // GET: Owner/List
        public ActionResult List()
        {
            string url = "OwnerData/ListOwners";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<OwnerDto> owner = response.Content.ReadAsAsync<IEnumerable<OwnerDto>>().Result;


            return View(owner);
        }

        // GET: Owner/Details/5
        public ActionResult Details(int id)
        {
            OwnerDetails ViewModel = new OwnerDetails();

            string url = "OwnerData/FindOwner/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            OwnerDto owner = response.Content.ReadAsAsync<OwnerDto>().Result;
            ViewModel.Owner = owner;

            url = "housedata/ListHousesForOwners/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<HouseDto> RelatedHouses = response.Content.ReadAsAsync<IEnumerable<HouseDto>>().Result;
            ViewModel.RelatedHouses = RelatedHouses;

            return View(ViewModel);
        }

        // GET: Owner/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        public ActionResult Create(Owner Owner)
        {
            string url = "OwnerData/addowner";

            string jsonpayload = jss.Serialize(Owner);

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

        // GET: Owner/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "ownerdata/findowner/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            OwnerDto Owner = response.Content.ReadAsAsync<OwnerDto>().Result;
            return View(Owner);
        }

        // POST: Owner/Update/5
        [HttpPost]
        public ActionResult Update(int id, Owner Owner)
        {
            string url = "ownerdata/updateowner/" + id;
            string jsonpayload = jss.Serialize(Owner);
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

        // GET: Owner/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "OwnerData/findOwner/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            OwnerDto Owner = response.Content.ReadAsAsync<OwnerDto>().Result;


            return View(Owner);
        }
        public ActionResult Error()
        {
            return View();
        }

        // POST: Owner/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Owner Owner)
        {
            string url = "OwnerData/deleteOwner/" + id;
            string jsonpayload = jss.Serialize(Owner);

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
