using Calendar.App.Data;
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

        public DataService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task ReserveDate(DateTime date, string userId)
        {
            var dateToReserve = db.Dates
                .Where(d => d.ReservedDate == date).FirstOrDefault();

            dateToReserve.UserId = userId;
            dateToReserve.IsReserved = true;
            
            await db.SaveChangesAsync();
        }

        public async Task AddAvailableDate(DateTime date, decimal price)
        {
            var availableDate = new Date
            {
                ReservedDate = date,
                Price = price,
            };

            await db.Dates.AddAsync(availableDate);
            await db.SaveChangesAsync();
        }
    }
}
