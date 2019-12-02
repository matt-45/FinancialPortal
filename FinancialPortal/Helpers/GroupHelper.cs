using FinancialPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace FinancialPortal.Helpers
{
    public class GroupHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void CreateDefaultBudgetsForGroup(int groupId)
        {
            var group = db.Groups.Find(groupId);

            Budget food = new Budget
            {
                Name = "Food",
                GroupId = groupId
            };

            Budget utilities = new Budget
            {
                Name = "Utilities",
                GroupId = groupId
            };

            Budget entertainment = new Budget
            {
                Name = "Entertainment",
                GroupId = groupId
            };

            db.SaveChanges();

            BudgetItem restaurant = new BudgetItem
            {
                Name = "Restaurant",
                BudgetId = food.Id
            };

            BudgetItem fastFood = new BudgetItem
            {
                Name = "Fast Food",
                BudgetId = food.Id
            };

            BudgetItem groceries = new BudgetItem
            {
                Name = "Groceries",
                BudgetId = food.Id
            };

            db.SaveChanges();

            BudgetItem electricity = new BudgetItem
            {
                Name = "Electricity",
                BudgetId = utilities.Id
            };

            BudgetItem gas = new BudgetItem
            {
                Name = "Gas",
                BudgetId = utilities.Id
            };

            BudgetItem water = new BudgetItem
            {
                Name = "Water",
                BudgetId = utilities.Id
            };

            db.SaveChanges();

            BudgetItem internet = new BudgetItem
            {
                Name = "Cable and Internet",
                BudgetId = entertainment.Id
            };

            db.SaveChanges();

            db.BudgetItems.Add(groceries);
            db.BudgetItems.Add(fastFood);
            db.BudgetItems.Add(restaurant);
            db.BudgetItems.Add(gas);
            db.BudgetItems.Add(water);
            db.BudgetItems.Add(electricity);
            db.BudgetItems.Add(internet);

            db.Budgets.Add(food);
            db.Budgets.Add(utilities);
            db.Budgets.Add(entertainment);

            db.SaveChanges();

            food.BudgetItems.Add(groceries);
            food.BudgetItems.Add(fastFood);
            food.BudgetItems.Add(restaurant);

            utilities.BudgetItems.Add(gas);
            utilities.BudgetItems.Add(water);
            utilities.BudgetItems.Add(electricity);

            entertainment.BudgetItems.Add(internet);

            db.SaveChanges();

            group.Budgets.Add(food);
            group.Budgets.Add(utilities);
            group.Budgets.Add(entertainment);

            db.SaveChanges();
        }
    }
}