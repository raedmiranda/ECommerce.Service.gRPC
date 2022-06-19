using ECommerce.Service;
using ECommerce.Services.GRPCClient.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Services.GRPCClient.Controllers
{
    public class CustomersController : Controller
    {
        private CustomerManager.CustomerManagerClient customerManagerClient;

        public async Task<IActionResult> Index()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7006");
            customerManagerClient = new CustomerManager.CustomerManagerClient(canal);

            var request = new Empty();
            var response = await customerManagerClient.GetAllAsync(request);

            List<ClientModel> models = new List<ClientModel>();
            foreach (var model in response.Items)
                models.Add(new ClientModel()
                {
                    Id = int.Parse(model.Id),
                    Name = model.Name,
                    Dob = DateTime.Parse(model.Dob),
                });

            return View(models);
        }
    }
}
