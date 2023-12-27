using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Maliban.Pos.Api.Repository
{
    public class EFUserRepository : RepositoryBase<ApplicationUser>, IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISortHelper<ApplicationUser> _sortHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EFUserRepository(DataContext ctx, ISortHelper<ApplicationUser> sortHelper, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : base(ctx)
        {
            context = ctx;
            _sortHelper = sortHelper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<PagedList<ApplicationUser>> GetUsersAsync(UserParams userParams, int? branchLocationId)
        {
            // get header data (on branch location id)
            string header = _httpContextAccessor.HttpContext.Request.Headers["BranchLocationId"];
            var user = await _userManager.Users.Where(u => u.Id == userParams.Id && u.Active == true).FirstOrDefaultAsync();
            if(await _userManager.IsInRoleAsync(user, "GlobalAdministrator"))
            {
                var userList = _userManager.Users.Where(u => u.Active == true).Include(r => r.UserRoles).ThenInclude(u => u.ApplicationRole).AsNoTracking();
                return await PagedList<ApplicationUser>.CreateAsync(userList, userParams.PageNumber, userParams.PageSize);

            }
            var users = _userManager.Users.Where(u => u.BranchLocationId == branchLocationId).Include(r => r.UserRoles).ThenInclude(u => u.ApplicationRole).AsNoTracking();

            if (users.Count() > 0)
            {
                //_sortHelper.SearchByName(ref suppliers,userParams.Filter);
                _sortHelper.ApplySort(ref users, userParams.OrderBy);
                SearchByName(ref users, userParams.Filter);
                //ApplySort(ref suppliers, userParams.OrderBy);
            }
            return await PagedList<ApplicationUser>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        public async Task<ApplicationUser> GetUserAsync(int userId) =>
         await _userManager.Users.Where(u => u.Id == userId).Include(r => r.UserRoles).SingleAsync<ApplicationUser>();

        private void SearchByName(ref IQueryable<ApplicationUser> suppliers, string supplierName)
        {
            if (!suppliers.Any() || string.IsNullOrWhiteSpace(supplierName) || supplierName=="undefined")
                return;

            suppliers = suppliers.Where(o => o.UserName.ToLower().Contains(supplierName.Trim().ToLower()));
        }

    }
    //https://code-maze.com/sorting-aspnet-core-webapi/
}