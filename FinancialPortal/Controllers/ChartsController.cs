using FinancialPortal.Models;
using FinancialPortal.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPortal.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Charts
        public PartialViewResult LoadChartData(int groupId)
        {
            var group = db.Groups.Find(groupId);
            var baseAmount = group.StartAmount;

            var data = new ChartData();
            for (int i = 1; i < 31; i++)
            {
                data.Labels.Add($"{i}");
            }
            
            for (int i = 1; i < 31; i++)
            {
                data.Data.Add(baseAmount);

            }
            return PartialView("", data);
            
        }
    }
}