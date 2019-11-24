using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Memo { get; set; }
        public DateTime Created { get; set; }
        public TransactionType Type { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        public int BudgetItemId { get; set; }
        public BudgetItem BudgetItem { get; set; }

        public int BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal
    }
        
}