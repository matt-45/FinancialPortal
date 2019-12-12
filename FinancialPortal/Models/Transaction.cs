using FinancialPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Transaction
    {

        ApplicationDbContext db = new ApplicationDbContext();
        NotificationHelper notificationHelper = new NotificationHelper();

        public int Id { get; set; }
        public decimal Amount { get; set; }
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
            var budget = db.Budgets.Find(BudgetId);
            var budgetItem = db.BudgetItems.Find(BudgetItemId);
            bank.Balance -= Amount;
            group.Balance -= Amount;
            budget.Spent += Amount;
            budgetItem.Spent += Amount;
            notificationHelper.CheckOverdrafts(Id);
            db.SaveChanges();
        }

    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
        
}