using Calendar.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calendar.App.Services
{
    public interface IDataService
    {
        Task<ICollection<ReservationViewModel>> ShowAllReservations();

        Task ReleaseReservation(string reservationId);

        Task AddAvailableDate(DateTime date, bool IsNonWorkDay, string userId);

        Task ChangePrices(decimal workday, decimal weekends);

        bool IsNonWorkDay(DateTime date);

        Task<ICollection<ReservationViewModel>> GetDates(int year, int month);
    }
}
