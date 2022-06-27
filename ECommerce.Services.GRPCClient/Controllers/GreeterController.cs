using Microsoft.AspNetCore.Mvc;
using ECommerce.Service;
using Grpc.Net.Client;

namespace ECommerce.Service.GRPCClient.Controllers
{
    public class GreeterController : Controller
    {
        private Greeter.GreeterClient greeterClient;

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string nombre="", string apellido = "")
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7006");
            greeterClient = new Greeter.GreeterClient(canal);

            var helloRequest = new HelloRequest();
            helloRequest.Name = nombre;
            helloRequest.Lastname = apellido;

            var response = await greeterClient.SayHelloAsync(helloRequest);
            ViewBag.respuesta = response.Message;
            return View();
        }
    }
}
