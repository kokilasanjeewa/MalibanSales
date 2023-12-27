using System;
namespace Maliban.Pos.Api.Helper
{
  // add abstract class for inherit
  public class UserParams
  {
    private const int MaxPageSize = 50;
    private int pageSize = 5;
    public int PageNumber { get; set; } = 1;
    public int PageSize
    {
      get { return pageSize; }
      set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
    }
    public string Filter { get; set; }
    public string OrderBy { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool Status { get; set; }
    public int? Id { get; set; }
    public int? SchemeCode { get; set; }
    public string POListCriteria { get; set; }
    }
    public class POListCriteria
    {
        public DateTime? POCreateFromDate { get; set; }
        public DateTime? POCreateToDate { get; set; }
        public string ProductId { get; set; }
        public string SupplierId { get; set; }
        public string BranchLocationId { get; set; }
        public DateTime? DeliveryFromDate { get; set; }
        public DateTime? DeliveryToDate { get; set; }
        public string filterby { get; set; } 
    }
}