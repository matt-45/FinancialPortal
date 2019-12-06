using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Spent { get; set; }
        public decimal Target { get; set; }


        [NotMapped]
        public double Percentage
        {
            get
            {
                var target = Decimal.ToDouble(Target);
                var spent = Decimal.ToDouble(Spent);

                if (Target == 0)
                {
                    return 0;
                }
                else
                {
                    return Math.Round(spent / target * 100);
                }
            }
        }

        [NotMapped]
        public decimal TotalItemTarget
        {
            get
            {
                decimal total = 0;
                foreach (var item in BudgetItems)
                {
                    total += item.Target;
                }
                return total;
            }
        }

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