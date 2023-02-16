using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Core.Specifications;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net;

namespace MobyLabWebProgramming.Infrastructure.Services.Implementations;

public class UserFileService : IUserFileService
{
    private readonly IRepository<WebAppDatabaseContext> _repository;
    private readonly IFileRepository _fileRepository;

    private static string GetFileDirectory(Guid userId) => Path.Join(userId.ToString(), IUserFileService.UserFilesDirectory);

    public UserFileService(IRepository<WebAppDatabaseContext> repository, IFileRepository fileRepository)
    {
        _repository = repository;
        _fileRepository = fileRepository;
    }

    public async Task<ServiceResponse<PagedResponse<UserFileDTO>>> GetUserFiles(PaginationSearchQueryParams pagination, CancellationToken cancellationToken = default)
    {
        var result = await _repository.PageAsync(pagination, new UserFileProjectionSpec(pagination.Search), cancellationToken);

        return ServiceResponse<PagedResponse<UserFileDTO>>.ForSuccess(result);
    }

    public async Task<ServiceResponse> SaveFile(UserFileAddDTO file, UserDTO requestingUser, CancellationToken cancellationToken = default)
    {
        var fileName = _fileRepository.SaveFile(file.File, GetFileDirectory(requestingUser.Id));

        if (fileName.Result == null)
        {
            return fileName.ToResponse();
        }

        await _repository.AddAsync(new UserFile
        {
            Name = file.File.FileName,
            Description = file.Description,
            Path = fileName.Result,
            UserId = requestingUser.Id
        }, cancellationToken);

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse<FileDTO>> GetFileDownload(Guid id, CancellationToken cancellationToken = default)
    {
        var userFile = await _repository.GetAsync<UserFile>(id, cancellationToken);

        return userFile != null ? 
            _fileRepository.GetFile(Path.Join(GetFileDirectory(userFile.UserId), userFile.Path), userFile.Name) : 
            ServiceResponse<FileDTO>.FromError(new(HttpStatusCode.NotFound, "File entry not found!", ErrorCodes.EntityNotFound));
    }
}
