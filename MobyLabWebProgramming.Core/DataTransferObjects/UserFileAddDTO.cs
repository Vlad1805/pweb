using Microsoft.AspNetCore.Http;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class UserFileAddDTO
{
    public IFormFile File { get; set; } = default!;
    public string? Description { get; set; }
}
