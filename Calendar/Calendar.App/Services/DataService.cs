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
    public class DataService : IDataService
    {
        private ApplicationDbContext db;
        private readonly IPriceService priceService;

        public DataService(ApplicationDbContext db, IPriceService priceService)
        {
            this.db = db;
            this.priceService = priceService;
        }

        public async Task<IEnumerable<AllReservationViewModel>> ShowAllReservations()
        {
            var reservations = await db.Dates.Select(p => new AllReservationViewModel
            {
                UserId = p.UserId,
                Username = db.Users.Where(u => u.Id == p.UserId).Select(u => u.UserName).FirstOrDefault(),
                ReservedDate = p.ReservedDate.ToString(),
                Price = p.Price.ToString(),
            }).ToListAsync();

            return reservations;
        }

        public async Task ReserveDate(DateTime date, string userId)
        {
            var dateToReserve = db.Dates
                .Where(d => d.ReservedDate == date).FirstOrDefault();

            dateToReserve.UserId = userId;
            dateToReserve.IsReserved = true;
            
            await db.SaveChangesAsync();
        }

        public async Task AddAvailableDate(DateTime date, bool isNonWorkDay, string userId)
        {
            var actualPrice = await priceService.ReturnActualPrice(isNonWorkDay);

            var availableDate = new Date
            {
                ReservedDate = date,
                Price = actualPrice,
                UserId = userId,
                IsNonWorkDay = isNonWorkDay,
            };

            await db.Dates.AddAsync(availableDate);
            await db.SaveChangesAsync();
        }

        public bool IsNonWorkDay(DateTime date)
        {
            var day = date.DayOfWeek;
            bool IsNonWorkDay = false;

            if (day == DayOfWeek.Sunday || day == DayOfWeek.Saturday)
            {
                IsNonWorkDay = true;
            }

            return IsNonWorkDay;
        }

        public async Task ChangePrices(decimal workday, decimal weekends)
        {
            var prices = await db.Prices.FirstOrDefaultAsync();

            if (prices == null)
            {
                prices = new Price
                {
                    WorkDay = workday,
                    NonWorkDay = weekends,
                };

                await db.Prices.AddAsync(prices);
            }
            else
            {
                prices.WorkDay = workday;
                prices.NonWorkDay = weekends;
            }

            await db.SaveChangesAsync();
        }
    }
}
