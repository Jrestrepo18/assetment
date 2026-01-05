namespace OnlineCoursesPlatform.Web.Models;

/// <summary>
/// ViewModel para paginación de resultados.
/// </summary>
public class PaginationViewModel
{
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }
    
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    
    public int FirstItemIndex => (CurrentPage - 1) * PageSize + 1;
    public int LastItemIndex => Math.Min(CurrentPage * PageSize, TotalItems);

    public IEnumerable<int> PageNumbers
    {
        get
        {
            var startPage = Math.Max(1, CurrentPage - 2);
            var endPage = Math.Min(TotalPages, CurrentPage + 2);
            
            for (int i = startPage; i <= endPage; i++)
            {
                yield return i;
            }
        }
    }
}
