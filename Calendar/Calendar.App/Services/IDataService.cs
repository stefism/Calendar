using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.App.Services
{
    public interface IDataService
    {
        Task ReserveDate(DateTime date, string userId);

        Task AddAvailableDate(DateTime date, decimal price);
    }
}
