using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IUserFileService
{
    public const string UserFilesDirectory = "UserFiles";

    public Task<ServiceResponse<PagedResponse<UserFileDTO>>> GetUserFiles(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default);
    public Task<ServiceResponse> SaveFile(UserFileAddDTO file, UserDTO requestingUser, CancellationToken cancellationToken = default);
    public Task<ServiceResponse<FileDTO>> GetFileDownload(Guid id, CancellationToken cancellationToken = default);
}
