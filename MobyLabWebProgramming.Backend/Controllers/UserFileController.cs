using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Extensions;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;
using System.Net.Mime;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserFileController : AuthorizedController
{
    private readonly IUserFileService _userFileService;

    public UserFileController(IUserService userService, IUserFileService userFileService) : base(userService)
    {
        _userFileService = userFileService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<UserFileDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _userFileService.GetUserFiles(pagination)) :
            this.ErrorMessageResult<PagedResponse<UserFileDTO>>(currentUser.Error);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromForm] UserFileAddDTO form)
    {
        var currentUser = await GetCurrentUser();

        return currentUser.Result != null ?
            this.FromServiceResponse(await _userFileService.SaveFile(form, currentUser.Result)) :
            this.ErrorMessageResult(currentUser.Error);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [Produces(MediaTypeNames.Application.Octet, MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse>> Download([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();

        if (currentUser.Result == null)
        {
            return this.ErrorMessageResult(currentUser.Error);
        }

        var file = await _userFileService.GetFileDownload(id);

        return file.Result != null ? 
            File(file.Result.Stream, MediaTypeNames.Application.Octet, file.Result.Name) : 
            this.ErrorMessageResult(currentUser.Error);
    }
}
