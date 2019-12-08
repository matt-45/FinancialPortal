using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class TransactionEditViewModel
    {
        public ApplicationUser User { get; set; }
        public Group Group { get; set; }
        public Transaction Transaction { get; set; }
        public Budget Budget { get; set; }
    }
}