using FluentTranslate.WebApi.Jwt;

namespace FluentTranslate.WebApi.Services
{
    public interface IAuthenticationService
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserCommand command);
        Task<LoginUserResponse> LoginUserAsync(LoginUserCommand command);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenProvider _jwtProvider;

        private List<User> _users = new();

        public AuthenticationService(
            IPasswordHasher passwordHasher, 
            IJwtTokenProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                throw new ArgumentException("The provided email is blank.");
            if (string.IsNullOrWhiteSpace(command.UserName))
                throw new ArgumentException("The provided user name is blank.");
            if (string.IsNullOrWhiteSpace(command.Password))
                throw new ArgumentException("The provided password is blank.");

            var hash = _passwordHasher.Hash(command.Password);
            var user = new User() { Email = command.Email, Name = command.UserName, PasswordHash = hash };
            _users.Add(user);

            var token = await LoginUserAsync(user, command.Password);

            return new RegisterUserResponse(token);
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserName))
                throw new ArgumentException("The provided user name is blank.");
            if (string.IsNullOrWhiteSpace(command.Password))
                throw new ArgumentException("The provided password is blank.");

            var user = _users.FirstOrDefault(x => x.Name == command.UserName);
            var token = await LoginUserAsync(user, command.Password);
            return new LoginUserResponse(token);
        }

        private async Task<string> LoginUserAsync(User? user, string password)
        {
            var timeout = Task.Delay(100);
            if (user is null || !_passwordHasher.Verify(user.PasswordHash, password))
            {
                await timeout;
                throw new ArgumentException("The provided user name or password is incorrect.");
            }

            var token = _jwtProvider.CreateToken(user);
            await timeout;
            return token;
        }
    }
}
