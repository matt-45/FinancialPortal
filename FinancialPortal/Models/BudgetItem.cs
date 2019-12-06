using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Spent { get; set; }
        public decimal Target { get; set; }


        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

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

        public virtual ICollection<Transaction> Transactions { get; set; }

        public BudgetItem()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}