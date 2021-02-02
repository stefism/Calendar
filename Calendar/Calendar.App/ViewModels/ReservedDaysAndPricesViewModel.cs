using System.Collections.Generic;

namespace Calendar.App.ViewModels
{
    public class ReservedDaysAndPricesViewModel
    {
        public PricesViewModel Prices { get; set; }
        public ICollection<ReservationViewModel> ReservedDays { get; set; } = new List<ReservationViewModel>();
    }
}
