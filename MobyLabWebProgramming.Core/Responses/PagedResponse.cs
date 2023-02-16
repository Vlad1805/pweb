namespace MobyLabWebProgramming.Core.Responses;

public class PagedResponse<T>
{
    public uint Page { get; set; }
    public uint PageSize { get; set; }
    public uint TotalCount { get; set; }
    public List<T> Data { get; set; }

    public PagedResponse(uint page, uint pageSize, uint totalCount, List<T> data)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        Data = data;
    }
}