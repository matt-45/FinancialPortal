using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal StartAmount { get; set; }

        [NotMapped]
        public decimal TotalBudgetTarget
        {
            get
            {
                decimal total = 0;
                foreach (var budget in Budgets)
                {
                    total += budget.Target;
                }
                return total;
            }
        }

        [NotMapped]
        public decimal Spent { 
            get 
            {
                return StartAmount - Balance;
            } 
        }

        [NotMapped]
        public decimal EstimatedBalance
        {
            get
            {
                return StartAmount - Transactions.Sum(t => t.Amount);
            }
        }

        [NotMapped]
        public double Percentage
        {
            get
            {
                var target = Decimal.ToDouble(StartAmount);
                var spent = Decimal.ToDouble(Spent);

                if (target == 0)
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
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }

        public Group()
        {
            Transactions = new HashSet<Transaction>();
            Notifications = new HashSet<Notification>();
            Users = new HashSet<ApplicationUser>();
            Budgets = new HashSet<Budget>();
            Invitations = new HashSet<Invitation>();
        }

    }
}