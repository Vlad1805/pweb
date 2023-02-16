namespace MobyLabWebProgramming.Infrastructure.Authorization;

public record UserClaims(Guid Id, string? Name, string? Email);
