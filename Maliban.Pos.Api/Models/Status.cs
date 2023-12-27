namespace Maliban.Pos.Api.Models
{
  public static class Status
  {
    public enum StatusType { Add, Change, TypingMistake }
    public enum TaXMethod { NOVAT, SVAT, VAT }
    // public enum AdjustmentType { damage, expires, destroyed, stockadjustment }
    public enum ReorderLevel { Zero, Available, OrderPending }
    public enum PurchaseRequestStatus { Unapproved, Approved, Authorized }

  }
}