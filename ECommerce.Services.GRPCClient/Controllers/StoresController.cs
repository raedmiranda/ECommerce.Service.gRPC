using ECommerce.Service;
using ECommerce.Service.Protos;
using ECommerce.Service.GRPCClient.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Service.GRPCClient.Controllers
{
    public class StoresController : Controller
    {
        private Service.Protos.StoreManager.StoreManagerClient storeManagerClient;

        // GET: StoresController
        public async Task<IActionResult> Index()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7006");
            storeManagerClient = new StoreManager.StoreManagerClient(canal);

            var request = new EmptyS();
            var response = await storeManagerClient.GetAllAsync(request);

            List<StoreModel> models = new List<StoreModel>();
            foreach (var model in response.Items)
                models.Add(new StoreModel()
                {
                    Id = int.Parse(model.Id),
                    Name = model.Name,
                    City = model.City
                });

            return View(models);
        }

        // GET: StoresController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7006");
            storeManagerClient = new StoreManager.StoreManagerClient(canal);

            var request = new StoreId();
            request.Id = id + "";
            var response = await storeManagerClient.GetAsync(request);

            StoreModel model = new StoreModel()
            {
                Id = int.Parse(response.Id),
                Name = response.Name,
                City = response.City
            };

            return View(model);
        }

        // GET: StoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreModel model)
        {
            try
            {
                var canal = GrpcChannel.ForAddress("https://localhost:7006");
                storeManagerClient = new StoreManager.StoreManagerClient(canal);

                var request = new Store();
                request.Id = model.Id + "";
                request.Name = model.Name;
                request.City = model.City;
                var response = await storeManagerClient.InsertAsync(request);

                if (response.StatusText == "OK")
                    return RedirectToAction(nameof(Index));
                else 
                    return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: StoresController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StoresController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoresController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
