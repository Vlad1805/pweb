using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Authorization;

public abstract class AuthorizedController : ControllerBase
{
    private UserClaims? _userClaims;
    protected readonly IUserService UserService;

    protected AuthorizedController(IUserService userService) => UserService = userService;

    protected UserClaims ExtractClaims()
    {
        if (_userClaims != null)
        {
            return _userClaims;
        }

        var enumerable = User.Claims.ToList();
        var userId = enumerable.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => Guid.Parse(x.Value)).FirstOrDefault();
        var email = enumerable.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
        var name = enumerable.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();

        _userClaims = new(userId, name, email);

        return _userClaims;
    }

    protected Task<ServiceResponse<UserDTO>> GetCurrentUser() => UserService.GetUser(ExtractClaims().Id);
}
