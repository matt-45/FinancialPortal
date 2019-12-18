using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class TransactionCreateViewModel
    {
        public ApplicationUser User { get; set; }
        public Group Group { get; set; }
        public Budget? Budget { get; set; }
        public string DateTime { get; set; }
    }
}