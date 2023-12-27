using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Repository;
using System;
using System.Linq;

namespace Maliban.Pos.Api.Helper
{
    public class Utility
    {
        private DataContext _context;

        private IRepositoryWrapper _repoWrapper;
        public Utility(IRepositoryWrapper repoWrapper,DataContext context)
        {
            _context = context;
            _repoWrapper = repoWrapper;
        }
        public decimal CreditBalanceCalculation(string number,int branchLocationId)
        {
            var customer = _repoWrapper.Employee.FindByCondition(c => c.Number == number).FirstOrDefault();
            var companySetting = _context.CustomerMasterSettings.Where(c => c.BranchLocationId == branchLocationId && c.CustomerCategory.Contains(customer.Scheme)).FirstOrDefault();
            decimal creditBalance = 0;
            var str = DateTime.Now.Month.ToString() + "/" + companySetting.BillingCycleStartDate + "/" + DateTime.Now.Year.ToString();
            var end = DateTime.Now.Month.ToString() + "/" + companySetting.BillingCycleEndDate + "/" + DateTime.Now.Year.ToString();
            DateTime dt;
            DateTime.TryParse(str, out dt);
            DateTime dt1;
            DateTime.TryParse(end, out dt1);
            //get difference of two dates
            var today = DateTime.Now.Date;
            var diffOfDates = today - dt;
            int days = diffOfDates.Days;
            if ((today >= dt && today <= dt1) && (companySetting.BillingCycleStartDate >1 && companySetting.BillingCycleEndDate>1))
            {
                creditBalance = (companySetting.CreaditBalancePerDays * days) + companySetting.CreaditBalancePerDays;
                return creditBalance;
            }
            creditBalance = customer.CreditLimit;
            return creditBalance;
        }
    }
}
