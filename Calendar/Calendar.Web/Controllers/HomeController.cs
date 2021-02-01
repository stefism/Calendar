using Calendar.App.Services;
using Calendar.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Calendar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IDataService dataService;
        private readonly IPriceService priceService;

        public HomeController(
            ILogger<HomeController> logger,
            RoleManager<IdentityRole> roleManager,
            IDataService dataService,
            IPriceService priceService)
        {
            _logger = logger;
            this.roleManager = roleManager;
            this.dataService = dataService;
            this.priceService = priceService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await priceService.ReturnPrices();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePrices(decimal workday, decimal weekends)
        {
            await dataService.ChangePrices(workday, weekends);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CreateAdminRole()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));

            return RedirectToAction(nameof(Privacy));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
