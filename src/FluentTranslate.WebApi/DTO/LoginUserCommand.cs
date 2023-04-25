namespace FluentTranslate.WebApi.DTO
{
    public record LoginUserCommand(
        string UserName,
        string Password
        );

    public record LoginUserResponse(
        string Token
        );
}
