namespace Shared.RequestFeatures;

public class ResourceParameters
{
    private const int maxPageSize = 10;

    public DateTime ProjectStart { get; set; }

    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 3;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > maxPageSize ? maxPageSize : value;
    }

    public string OrderBy { get; set; } = "ProjectStart";
}