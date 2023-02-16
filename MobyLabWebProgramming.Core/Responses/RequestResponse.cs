using MobyLabWebProgramming.Core.Errors;

namespace MobyLabWebProgramming.Core.Responses;

public class RequestResponse<T>
{
    public T? Response { get; private init; }
    public ErrorMessage? ErrorMessage { get; private init; }

    protected RequestResponse() { }

    public static RequestResponse FromError(ErrorMessage? error)
    {
        return error != null
            ? new RequestResponse
            {
                ErrorMessage = error
            }
            : new()
            {
                Response = "Ok"
            };
    }

    public static RequestResponse<string> FromServiceResponse(ServiceResponse serviceResponse)
    {
        return FromError(serviceResponse.Error);
    }

    public static RequestResponse<T> FromServiceResponse(ServiceResponse<T> serviceResponse)
    {
        return serviceResponse.Error != null
            ? new RequestResponse<T>
            {
                ErrorMessage = serviceResponse.Error
            }
            : new()
            {
                Response = serviceResponse.Result
            };
    }
}

public class RequestResponse : RequestResponse<string>
{
    public static RequestResponse OkRequestResponse => FromError(null);
}
