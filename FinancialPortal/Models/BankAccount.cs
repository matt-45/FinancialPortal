using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public AccountType Type { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
    public enum AccountType
    {
        Checking,
        Savings
    }
}