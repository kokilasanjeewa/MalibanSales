namespace Maliban.Pos.Api.Errors
{
  public class ApiResponse
  {
    public ApiResponse(int statusCode, string message = null)
    {
      StatusCode = statusCode;
      Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    private string GetDefaultMessageForStatusCode(int statusCode)
    {
      switch (statusCode)
      {
        case 400:
          return "A bad request, you have made";
        case 401:
          return "Authorized, you are not";
        case 404:
          return "Resource found, it was not";
        case 500:
          return "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change";
        default:
          return null;

      };
    }
  }
}