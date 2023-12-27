namespace Maliban.Pos.Api.Helper
{
  public class PaginationHeader
  {
    public int CurrentPage { set; get; }
    public int ItemsPerPage { set; get; } = 0;
    public int TotalItems { set; get; }
    public int TotalPages { set; get; }
    public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
      this.CurrentPage = currentPage;
      this.ItemsPerPage = itemsPerPage;
      this.TotalItems = totalItems;
      this.TotalPages = totalPages;
    }
  }
}