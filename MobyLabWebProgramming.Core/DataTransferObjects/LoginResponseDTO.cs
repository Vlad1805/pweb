namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class LoginResponseDTO
{
    public string Token { get; set; } = default!;
    public UserDTO User { get; set; } = default!;
}
