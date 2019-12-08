using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models.DataModels
{
    public class ChartData
    {
        public List<string> Labels {get; set;}
        public List<decimal> Data { get; set; }
    }
}