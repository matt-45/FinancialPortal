using FinancialPortal.Models;
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
            viewModel.group = group;
            viewModel.spentToday = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-1)).Select(t => t.Amount).Sum();
            viewModel.spentWeek = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-7)).Select(t => t.Amount).Sum();
            viewModel.spentMonth = group.Transactions.Where(t => t.Created >= DateTime.Now.AddDays(-30)).Select(t => t.Amount).Sum();
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