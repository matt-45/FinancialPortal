using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class DashboardViewModel
    {
        public Group group { get; set; }
        public decimal spentToday { get; set; }
        public decimal spentWeek { get; set; }
        public decimal spentMonth { get; set; }
    }
}