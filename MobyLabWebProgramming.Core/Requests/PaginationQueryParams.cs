namespace MobyLabWebProgramming.Core.Requests;

public class PaginationQueryParams
{
    public uint Page { get; set; } = 1;
    public uint PageSize { get; set; } = 10;
}