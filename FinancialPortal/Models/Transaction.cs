using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Transaction
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Memo { get; set; }
        public DateTime Created { get; set; }
        public TransactionType Type { get; set; }

        public string CreatorId { get; set; }
        public virtual ApplicationUser Creator { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public int? BudgetId { get; set; }
        public virtual Budget Budget { get; set; }


        public int? BudgetItemId { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }

        public int BankAccountId { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        public void Calculate()
        {
            var bank = db.BankAccounts.Find(BankAccountId);
            var group = db.Groups.Find(GroupId);
            bank.Balance -= Amount;
            group.Balance -= Amount;
            // check to see if there is an overdraft etc.
            db.SaveChanges();
        }

    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
        
}