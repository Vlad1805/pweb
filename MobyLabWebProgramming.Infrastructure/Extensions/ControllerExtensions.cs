using System.Net;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Extensions;

public static class ControllerExtensions
{
    public static ActionResult<RequestResponse> ErrorMessageResult(this ControllerBase controller, ServerException serverException) =>
        controller.StatusCode((int)serverException.Status, RequestResponse.FromError(ErrorMessage.FromException(serverException)));

    public static ActionResult<RequestResponse> ErrorMessageResult(this ControllerBase controller, ErrorMessage? errorMessage = default) =>
        controller.StatusCode((int)(errorMessage?.Status ?? HttpStatusCode.InternalServerError), RequestResponse.FromError(errorMessage));

    public static ActionResult<RequestResponse<T>> ErrorMessageResult<T>(this ControllerBase controller, ErrorMessage? errorMessage = default) =>
        controller.StatusCode((int)(errorMessage?.Status ?? HttpStatusCode.InternalServerError), RequestResponse<T>.FromError(errorMessage));

    public static ActionResult<RequestResponse> FromServiceResponse(this ControllerBase controller, ServiceResponse response) =>
        response.Error == null ? controller.Ok(RequestResponse.OkRequestResponse) : controller.ErrorMessageResult(response.Error);

    public static ActionResult<RequestResponse<T>> FromServiceResponse<T>(this ControllerBase controller, ServiceResponse<T> response) =>
        response.Error == null ? controller.Ok(RequestResponse<T>.FromServiceResponse(response)) : controller.ErrorMessageResult<T>(response.Error);

    public static ActionResult<RequestResponse> OkRequestResponse(this ControllerBase controller) => controller.Ok(RequestResponse.OkRequestResponse);
}
