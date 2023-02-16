using System.Text.Json.Serialization;

namespace MobyLabWebProgramming.Core.Errors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorCodes
{
    Unknown,
    TechnicalError,
    EntityNotFound,
    PhysicalFileNotFound,
    UserAlreadyExists,
    EmailAlreadyInUse,
    WrongPassword,
    CannotAdd,
    CannotUpdate,
    CannotDelete
}
