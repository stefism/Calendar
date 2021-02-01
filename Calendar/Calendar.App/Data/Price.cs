using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Calendar.App.Data
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        public decimal WorkDay { get; set; }

        public decimal NonWorkDay { get; set; }
    }
}
