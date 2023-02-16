using Microsoft.AspNetCore.Http;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Repositories.Interfaces;

public interface IFileRepository
{
    public ServiceResponse<FileDTO> GetFile(string filePath, string? replacedFileName = default);
    public ServiceResponse<FileDTO> SaveFileAndGet(IFormFile file, string directoryPath);
    public ServiceResponse<string> SaveFile(IFormFile file, string directoryPath);
    public ServiceResponse<FileDTO> SaveFileAndGet(byte[] file, string directoryPath, string fileExtension);
    public ServiceResponse<string> SaveFile(byte[] file, string directoryPath, string fileExtension);
    public ServiceResponse<FileDTO> UpdateFileAndGet(IFormFile file, string filePath);
    public ServiceResponse<string> UpdateFile(IFormFile file, string filePath);
    public ServiceResponse<FileDTO> UpdateFileAndGet(byte[] file, string filePath);
    public ServiceResponse<string> UpdateFile(byte[] file, string filePath);
    public ServiceResponse DeleteFile(string filePath);
}
