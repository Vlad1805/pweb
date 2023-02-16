using MobyLabWebProgramming.Core.DataTransferObjects;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ILoginService
{
    public string GetToken(UserDTO user, DateTime issuedAt, TimeSpan expiresIn);
}
