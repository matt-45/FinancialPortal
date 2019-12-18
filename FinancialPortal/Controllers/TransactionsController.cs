using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.BankAccount).Include(t => t.BudgetItem).Include(t => t.Creator);
            return View(transactions.ToList());
        }
        public ActionResult Create(int? budgetId)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var viewModel = new TransactionCreateViewModel();
            if (budgetId != null)
            {
                viewModel.Budget = db.Budgets.Find(budgetId);
            }
            viewModel.User = user;
            viewModel.Group = user.Group;
            viewModel.DateTime = DateTime.Now.ToString("MM/dd/yyyy");
            return View(viewModel);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int budgetItemId, int bankAccountId, decimal amount, string memo)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var item = db.BudgetItems.Find(budgetItemId);
            var bankAccount = db.BankAccounts.Find(bankAccountId);
            var budget = item.Budget;

            var transaction = new Transaction
            {
                Amount = amount,
                Memo = memo,
                Type = TransactionType.Withdrawal,
                Created = DateTime.Now,
                CreatorId = user.Id,
                GroupId = (int)user.GroupId,
                BudgetId = budget.Id,
                BudgetItemId = budgetItemId,
                BankAccountId = bankAccountId
            };
            
            db.Transactions.Add(transaction);
            db.SaveChanges();
            user.Group.Transactions.Add(transaction);
            bankAccount.Transactions.Add(transaction);

            transaction.Calculate();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateDeposit(int bankId, decimal amount, string memo) // bankid, amount
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var group = user.Group;
            var account = db.BankAccounts.Find(bankId);
            var transaction = new Transaction
            {
                Memo = memo,
                Amount = amount,
                BankAccountId = bankId,
                Type = TransactionType.Deposit,
                Created = DateTime.Now,
                GroupId = (int)user.GroupId,
                CreatorId = user.Id
            };
            account.Balance += amount;
            db.Transactions.Add(transaction);
            db.SaveChanges();

            return PartialView("~/Views/Shared/_DashboardPartialTwo.cshtml", group);
        }

        // GET: Transactions/Edit/5
        public ActionResult Details(int id, int? budgetId)
        {
            var transaction = db.Transactions.Find(id);
            var user = db.Users.Find(User.Identity.GetUserId());
            var group = db.Groups.Find(user.GroupId);
            
            
            var viewModel = new TransactionEditViewModel
            {
                Group = group,
                User = user,
                Transaction = transaction
            };
            if (budgetId == null)
            {
                viewModel.Budget = transaction.Budget;
            }
            else
            {
                viewModel.Budget = db.Budgets.Find(budgetId);
            }

            return View(viewModel);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int transactionId, int budgetItemId, int budgetId, int bankAccountId, decimal amount, string memo)
        {
            var transaction = db.Transactions.Find(transactionId);
            var bank = db.BankAccounts.Find(transaction.BankAccountId);
            var group = db.Groups.Find(transaction.GroupId);
            var budget = db.Budgets.Find(transaction.BudgetId);
            var budgetItem = db.BudgetItems.Find(transaction.BudgetItemId);

            bank.Balance += transaction.Amount;
            group.Balance += transaction.Amount;
            budget.Spent -= transaction.Amount;
            budgetItem.Spent -= transaction.Amount;

            transaction.BudgetItemId = budgetItemId;
            transaction.BudgetId = budgetId;
            transaction.Amount = amount;
            transaction.Memo = memo;
            transaction.BankAccountId = bankAccountId;

            transaction.Calculate();

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int id)
        {
            var transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);

            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
