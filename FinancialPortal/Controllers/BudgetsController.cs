using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;

namespace FinancialPortal.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index(int id)
        {
            var budget = db.Budgets.Find(id);
            return View(budget);
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int id)
        {
            var budget = db.Budgets.Find(id);
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.GroupId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string BudgetName, int GroupId)
        {
            var group = db.Groups.Find(GroupId);
            var budget = new Budget
            {
                Name = BudgetName,
                GroupId = GroupId
            };
            db.Budgets.Add(budget);
            group.Budgets.Add(budget);
            db.SaveChanges();
            return RedirectToAction("Details", "Budgets", new { id = budget.Id});
        }



        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int budgetId, string budgetName, decimal spentAmount, decimal targetAmount)
        {
            var budget = db.Budgets.Find(budgetId);
            budget.Name = budgetName;
            budget.Spent = spentAmount;
            budget.Target = targetAmount;

            db.SaveChanges();

            return RedirectToAction("Details", "Budgets", new { id = budgetId });
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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
