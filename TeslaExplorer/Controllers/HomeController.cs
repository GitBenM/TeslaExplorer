using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TeslaExplorer.Api;
using TeslaExplorer.DataAccess;
using TeslaExplorer.Models;

namespace TeslaExplorer.Controllers
{
    public class HomeController : Controller
    {
        public static TeslaClient TeslaApi = new TeslaClient();

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            var authRequestDto = new AuthRequestDto
            {
                ClientId = TeslaClient.TESLA_CLIENT_ID,
                ClientSecret = TeslaClient.TESLA_CLIENT_SECRET,
                Email = model.EmailAddress,
                GrantType = "password",
                Password = model.Password
            };

            var authResponse = await TeslaApi.AuthRequest(authRequestDto);

            if (authResponse.IsSuccess)
            {
                TeslaApi.Client.SetAuthorizationHeader(authResponse.Result.AccessToken);
                return RedirectToAction("Vehicles");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> WakeUp(string id)
        {
            var wakeUpResponse = await TeslaApi.PostWakeUp(id);

            return RedirectToAction("Vehicle", new { id });
        }

        public async Task<IActionResult> Vehicle(string id)
        {
            var vehicleData = await TeslaApi.GetVehicleData(id);

            if (vehicleData.IsSuccess)
            {
                return View(new VehicleViewModel
                {
                    Vehicle = vehicleData.Result.Response
                });
            }

            return RedirectToAction("Vehicles");
        }

        [HttpPost]
        public async Task<IActionResult> SetChargeLimit(string id, int percent)
        {
            var chargeResponse = await TeslaApi.PostChargeLimit(id, percent);

            return RedirectToAction("Vehicle", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> StartCharging(string id)
        {
            var chargeResponse = await TeslaApi.PostChargeStart(id);

            return RedirectToAction("Vehicle", new { id });
        }
        [HttpPost]
        public async Task<IActionResult> StopCharging(string id)
        {
            var chargeResponse = await TeslaApi.PostChargeStop(id);

            return RedirectToAction("Vehicle", new { id });
        }

        /// <summary>
        /// TODO add authorization
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Vehicles()
        {
            var response = await TeslaApi.GetVehicles();

            if (!response.IsSuccess)
                return RedirectToAction("Index");

            return View(new VehiclesViewModel
            {
                Vehicles = response.Result.Response
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
