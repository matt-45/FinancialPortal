using FinancialPortal.Models;
using FinancialPortal.Models.DataModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var group = db.Groups.Find(user.GroupId);
            var viewModel = new DashboardViewModel();
            viewModel.Group = group;
            viewModel.SpentToday = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-1)).Select(t => t.Amount).Sum();
            viewModel.SpentWeek = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-7)).Select(t => t.Amount).Sum();
            viewModel.SpentMonth = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-30)).Select(t => t.Amount).Sum();

            var today = DateTime.Now;
            var startDay = DateTime.Now.AddDays(-today.Day + 1);
            var baseAmount = group.StartAmount;
            var labels = new List<string>();
            var values = new List<decimal>();

            for (int i = 1; i < 31; i++)
            {
                labels.Add($"{i}");
            }

            for (int i = 1; i <= today.Day; i++)
            {
                values.Add(baseAmount);
                var transactionAmount = group.Transactions.Where(t => t.Created.ToString("MM/dd/yyyy") == startDay.AddDays(i).ToString("MM/dd/yyyy")).Sum(t => t.Amount);
                baseAmount -= transactionAmount;
            }

            var data = new ChartData {
                Labels = labels,
                Data = values
            };

            viewModel.ChartData = data;

            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}