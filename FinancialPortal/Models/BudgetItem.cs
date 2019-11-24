using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }

        public int BudgetId { get; set; }
        public Budget Budget { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}