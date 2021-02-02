using Calendar.App.Data;
using Calendar.App.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.App.Services
{
    public class PriceService : IPriceService
    {
        private readonly ApplicationDbContext db;

        public PriceService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<decimal> ReturnActualPrice(bool isNonWorkDay)
        {
            var prices = await db.Prices.FirstOrDefaultAsync();
            decimal currentPrice;

            if (isNonWorkDay)
            {
                currentPrice = prices.NonWorkDay;
            }
            else
            {
                currentPrice = prices.WorkDay;
            }

            return currentPrice;
        }

        public string TotalAmount()
        {
            var totalSum = db.Dates.Select(d => d.Price).Sum();

            return totalSum.ToString();
        }

        public async Task<PricesViewModel> ReturnPrices()
        {
            var prices = await db.Prices.Select(p => new PricesViewModel
            {
                WorkDay = p.WorkDay,
                NonWorkDay = p.NonWorkDay,
                TotalAmount = TotalAmount(),
            }).FirstOrDefaultAsync();

            if (prices == null)
            {
                prices = new PricesViewModel
                {
                    WorkDay = 0,
                    NonWorkDay = 0,
                };
            }

            return prices;
        }
    }
}
