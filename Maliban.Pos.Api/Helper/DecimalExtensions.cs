namespace Maliban.Pos.Api.Helper
{
  public static class DecimalExtensions
  {
    public static decimal WithTwoDecimalPoints(this decimal val)
    {
      return decimal.Parse(val.ToString("0.00000"));// maliban 0.00
    }
  }
}