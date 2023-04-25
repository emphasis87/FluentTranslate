namespace FluentTranslate.WebApi.Jwt
{
    public interface IJwtTokenProvider
    {
        string CreateToken(User user);
    }

    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
