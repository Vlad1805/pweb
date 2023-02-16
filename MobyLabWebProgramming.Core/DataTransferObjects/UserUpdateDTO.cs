namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record UserUpdateDTO(Guid Id, string? Name = default, string? Password = default);
