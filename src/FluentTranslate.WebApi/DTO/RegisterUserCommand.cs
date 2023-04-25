namespace FluentTranslate.WebApi.DTO
{
    public record RegisterUserCommand(
        string UserName,
        string Email,
        string Password
        );

    public record RegisterUserResponse(
        string Token
        );
}
