using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPortal.Models
{
    public partial class ApplicationUser
    {
        public void setUserIncome()
        {
            var user = this;
            switch (user.IncomeType)
            {
                case IncomeType.Hourly:
                    user.IncomeAmount = ((user.IncomeAmount * 24) * 365) / 12;
                    user.IncomeType = IncomeType.Monthly;
                    break;
                case IncomeType.Daily:
                    user.IncomeAmount = (user.IncomeAmount * 365) / 12;
                    user.IncomeType = IncomeType.Monthly;
                    break;
                case IncomeType.Weekly:
                    user.IncomeAmount = (user.IncomeAmount * 52) / 12;
                    user.IncomeType = IncomeType.Monthly;
                    break;
                case IncomeType.BiWeekly:
                    user.IncomeAmount = (user.IncomeAmount * 26) / 12;
                    user.IncomeType = IncomeType.BiWeekly;
                    break;
                case IncomeType.SemiMonthly:
                    user.IncomeAmount = (user.IncomeAmount / 2);
                    user.IncomeType = IncomeType.SemiMonthly;
                    break;
                case IncomeType.Monthly:
                    break;
                case IncomeType.Annually:
                    user.IncomeAmount = (user.IncomeAmount / 12);
                    user.IncomeType = IncomeType.Annually;
                    break;
            }
        }
    }
}