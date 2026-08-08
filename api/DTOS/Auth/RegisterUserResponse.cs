namespace api.DTOS.Auth;

public class RegisterUserResponse
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string Token {get;set;} = null!;

}
