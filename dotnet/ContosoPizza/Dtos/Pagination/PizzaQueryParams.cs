namespace ContosoPizza.Dtos.Pagination;

public class PagedQueryParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "Price";   // or Name, etc.
    public string? SortDirection { get; set; } = "asc";  // "asc" or "desc"

}