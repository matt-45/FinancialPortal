using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class DashboardViewModel
    {
        public Group group { get; set; }
        public double spentToday { get; set; }
        public double spentWeek { get; set; }
        public double spentMonth { get; set; }
    }
}