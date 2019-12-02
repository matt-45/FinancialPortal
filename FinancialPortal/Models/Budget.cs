using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Name { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        
        public Budget()
        {
            BudgetItems = new HashSet<BudgetItem>();
            Transactions = new HashSet<Transaction>();
        }

    }
}