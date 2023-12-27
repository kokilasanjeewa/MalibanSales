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

namespace Maliban.Pos.Api.Repository
{
  public class EFReportRepository : RepositoryBase<Report>, IReportRepository
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EFReportRepository(DataContext ctx, IHttpContextAccessor httpContextAccessor) : base(ctx)
    {
      context = ctx;
      _httpContextAccessor = httpContextAccessor;
    }
    public IQueryable<Report> Report => context.Reports;

  }
}