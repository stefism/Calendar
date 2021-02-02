using Calendar.App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Calendar.App.ViewModels;

namespace Calendar.Web.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataService dataService;
        private readonly IPriceService priceService;

        public DataController(IDataService dataService, IPriceService priceService)
        {
            this.dataService = dataService;
            this.priceService = priceService;
        }

        [HttpPost]
        public IActionResult ReserveDate(DateTime date)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            dataService.ReserveDate(date, userId);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddAvailableDate(DateTime date)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isNonWorkDay = dataService.IsNonWorkDay(date);

            await dataService.AddAvailableDate(date, isNonWorkDay, userId);

            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> GetDates(int year, int month)
        {
            var prices = await priceService.ReturnPrices();
            var reservedDays = await dataService.GetDates(year, month);

            return new JsonResult(new ReservedDaysAndPricesViewModel
            {
                Prices = prices,
                ReservedDays = reservedDays
            });
        }

        public async Task<IActionResult> AllReservations()
        {
            var model = await dataService.ShowAllReservations();           

            return View(model);
        }

        public async Task<IActionResult> ReleaseReservaton(string reservationId)
        {
            await dataService.ReleaseReservation(reservationId);

            return RedirectToAction(nameof(AllReservations));
        }
    }
}
