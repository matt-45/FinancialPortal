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

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
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
            return View(viewModel);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int budgetItemId, int bankAccountId, double amount, string memo)
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
                GroupId = user.GroupId,
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

        public ActionResult CreateDeposit(int bankId, double amount, string memo) // bankid, amount
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
                GroupId = user.GroupId,
                CreatorId = user.Id
            };
            account.Balance += amount;
            db.Transactions.Add(transaction);
            db.SaveChanges();

            return PartialView("~/Views/Shared/_DashboardPartialTwo.cshtml", group);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "FirstName", transaction.CreatorId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount,Memo,Created,Type,CreatorId,BudgetItemId,BankAccountId")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankAccountId = new SelectList(db.BankAccounts, "Id", "Name", transaction.BankAccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.CreatorId = new SelectList(db.Users, "Id", "FirstName", transaction.CreatorId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
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
