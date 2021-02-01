﻿using Calendar.App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Calendar.Web.Controllers
{
    public class DataController : Controller
    {
        private readonly IDataService dataService;

        public DataController(IDataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpPost]
        public IActionResult ReserveDate(DateTime date)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            dataService.ReserveDate(date, userId);

            return Ok();
        }

        [HttpPost]
        public IActionResult AddAvailableDate(DateTime date, decimal price)
        {
            dataService.AddAvailableDate(date, price);

            return Ok();
        }
    }
}
