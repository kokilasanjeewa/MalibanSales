using System.Linq;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
    public class EFEmployeeRepository : RepositoryBase<MI_Employer_Employee>, IEmployeeRepository
    {
        public EFEmployeeRepository(DataContext ctx) : base(ctx)
        {
            context = ctx;
        }
        public IQueryable<MI_Employer_Employee> Employees => context.MI_Employer_Employees;

        public async Task<PagedList<MI_Employer_Employee>> GetEmployersAsync(UserParams userParams, int status)
        {
           var branchCode = await context.BranchLocations.Where(b => b.BranchLocationId == status).FirstOrDefaultAsync();

            switch (branchCode.BranchCode)
            {
                case "HO":
                    var employers = context.MI_Employer_Employees.Where(b => b.Number.Contains("HO") || b.Number.Contains("TR")).AsQueryable();
                    if (employers.Count() > 0)
                    {
                        SearchByEmployeeNumber(ref employers, userParams.Filter);
                    }
                    return await PagedList<MI_Employer_Employee>.CreateAsync(employers, userParams.PageNumber, userParams.PageSize);
                case "BD":
                    var employers1 = context.MI_Employer_Employees.Where(b => b.Number.Contains("WW") || b.Number.Contains("BD")).AsQueryable();
                    if (employers1.Count() > 0)
                    {
                        SearchByEmployeeNumber(ref employers1, userParams.Filter);
                    }
                    return await PagedList<MI_Employer_Employee>.CreateAsync(employers1, userParams.PageNumber, userParams.PageSize);
                default:
                    var employers2 = context.MI_Employer_Employees.Where(b => b.Number.Contains(branchCode.BranchCode)).AsQueryable();
                    if (employers2.Count() > 0)
                    {
                        SearchByEmployeeNumber(ref employers2, userParams.Filter);
                    }
                    return await PagedList<MI_Employer_Employee>.CreateAsync(employers2, userParams.PageNumber, userParams.PageSize);

            }

        }
        private void SearchByEmployeeNumber(ref IQueryable<MI_Employer_Employee> customers, string ePFNo)
        {
            if (!customers.Any() || string.IsNullOrWhiteSpace(ePFNo))
                return;

            customers = customers.Where(o => o.EPFNo.ToLower().Contains(ePFNo.Trim().ToLower()));
        }
        public async Task<MI_Employer_Employee> GetEmployeeAsync(string barcode)
        {
            MI_Employer_Employee product = await context.MI_Employer_Employees.Where(p => p.Deleted == 0)
            .FirstOrDefaultAsync(u => u.EPFNo == barcode);
            return product;
        }
    }
}