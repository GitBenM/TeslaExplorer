using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using TeslaExplorer.Models;

namespace TeslaExplorer.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.EmailAddress) || string.IsNullOrEmpty(model.Password))
                return View(model);

            var api = UserApiFactory.GetApi(model.EmailAddress);

            //Todo add feedback
            if(api == null)
                return View(model);

            if (await new AuthRequestFactory().GetAuthToken(api, model.EmailAddress, model.Password))
            {
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.EmailAddress) }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("username", model.EmailAddress);

                return RedirectToAction("Vehicles");
            }

            return View(model);
        }

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index");            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> WakeUp(string id)
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var wakeUpResponse = await api.PostWakeUp(id);

            return RedirectToAction("Vehicle", new { id });
        }

        [Authorize]
        public async Task<IActionResult> Vehicle(string id)
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var vehicleData = await api.GetVehicleData(id);

            if (vehicleData.IsSuccess)
            {
                return View(new VehicleViewModel
                {
                    Vehicle = vehicleData.Result.Response
                });
            }

            return RedirectToAction("Vehicles");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetChargeLimit(string id, int percent)
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var chargeResponse = await api.PostChargeLimit(id, percent);

            return RedirectToAction("Vehicle", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StartCharging(string id)
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var chargeResponse = await api.PostChargeStart(id);

            return RedirectToAction("Vehicle", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> StopCharging(string id)
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var chargeResponse = await api.PostChargeStop(id);

            return RedirectToAction("Vehicle", new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChargeToStorage(string id, int storageStateOfCharge = 60)
        {
            BackgroundJob.Schedule(() => CheckChargeState.CheckChargingStatus(HttpContext.Session.GetString("username"), id, storageStateOfCharge), TimeSpan.FromSeconds(10));

            return RedirectToAction("Vehicle", new { id });
        }


        [Authorize]
        public async Task<IActionResult> Vehicles()
        {
            var api = UserApiFactory.GetApi(HttpContext.Session.GetString("username"));

            var response = await api.GetVehicles();

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
