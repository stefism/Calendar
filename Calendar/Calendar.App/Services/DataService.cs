using Calendar.App.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.App.Services
{    
    public class DataService
    {
        private ApplicationDbContext db;

        public DataService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task ReserveDate(DateTime date, decimal price, string userID)
        {
            var reservedDate = new Date
            {
                ReservedDate = date,
                Price = price,
                UserId = userID,
            };

            await db.Dates.AddAsync(reservedDate);
            await db.SaveChangesAsync();
        }
    }
}
