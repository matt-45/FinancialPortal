using FinancialPortal.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class DashboardViewModel
    {
        public Group Group { get; set; }
        public ChartData ChartData { get; set; }
        public decimal SpentToday { get; set; }
        public decimal SpentWeek { get; set; }
        public decimal SpentMonth { get; set; }
    }
}