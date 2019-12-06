using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Helpers
{
    public class NotificationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void CheckOverdrafts(int transactionId)
        {
            var transaction = db.Transactions.Find(transactionId);
            var user = transaction.Creator;
            var bankAccount = transaction.BankAccount;
            var group = transaction.Group;
            var budget = transaction.Budget;
            var budgetItem = transaction.BudgetItem;

            if (group.Balance < 0)
            {
                var notification = new Notification
                {
                    Message = "Your group has gone over budget!",
                    GroupId = group.Id
                };
                group.Notifications.Add(notification);
            }
            if (budget.Spent > budget.Target)
            {
                var notification = new Notification
                {
                    Message = $"Your groups budget ({budget.Name}) has gone over budget!",
                    GroupId = group.Id
                };
                group.Notifications.Add(notification);
            }
            if (budgetItem.Spent > budgetItem.Target)
            {
                var notification = new Notification
                {
                    Message = $"Your groups budget item ({budgetItem.Name}) has gone over budget!",
                    GroupId = group.Id
                };
                group.Notifications.Add(notification);
            }
            if (bankAccount.Balance < 0)
            {
                var notification = new Notification
                {
                    Message = $"Your bank account ({bankAccount.Name}) has gone over budget!",
                    UserId = user.Id
                };
                user.Notifications.Add(notification);
            }
            db.SaveChanges();
        }
    }
}