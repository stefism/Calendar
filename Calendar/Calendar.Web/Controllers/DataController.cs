﻿using Calendar.App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Calendar.App.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> AddAvailableDate(DateTime date)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            bool isNonWorkDay = dataService.IsNonWorkDay(date);

            await dataService.AddAvailableDate(date, isNonWorkDay, userId);

            return Ok();
        }

        [HttpGet]
        public async Task<JsonResult> GetReservedDates(int year, int month)
        {
            var prices = await priceService.ReturnPrices();
            var reservedDays = await dataService.GetReservedDates(year, month);

            return new JsonResult(new ReservedDaysAndPricesViewModel
            {
                Prices = prices,
                ReservedDays = reservedDays
            });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AllReservations()
        {
            var model = await dataService.ShowAllReservations();           

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReleaseReservaton(string reservationId)
        {
            await dataService.ReleaseReservation(reservationId);

            return RedirectToAction(nameof(AllReservations));
        }

        [Authorize]
        public async Task<IActionResult> ReleaseUserReservaton(string reservationId)
        {
            await dataService.ReleaseReservation(reservationId);

            return RedirectToAction("Index", "Home");
        }
    }
}
