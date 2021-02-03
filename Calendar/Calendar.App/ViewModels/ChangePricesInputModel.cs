using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Calendar.App.ErrMessages;

namespace Calendar.App.ViewModels
{
    public class ChangePricesInputModel
    {
        [Range(0.00, double.MaxValue, ErrorMessage = ErrorMessages.InvalidPrice)]
        public decimal Workday { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = ErrorMessages.InvalidPrice)]
        public decimal Weekends { get; set; }
    }
}
