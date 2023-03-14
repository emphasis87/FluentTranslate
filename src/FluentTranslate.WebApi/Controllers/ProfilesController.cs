using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluentTranslate.WebApi.Controllers
{
    [ApiController]
    [Route("")]
    public class ProfilesController : ControllerBase
    {

        private readonly ILogger<ProfilesController>? _logger;

        public ProfilesController(ILogger<ProfilesController>? logger = null)
        {
            _logger = logger;
        }

        [HttpGet("/profiles")]
        public IEnumerable<string> GetProfiles()
        {
            return new[] { "a", "b" };
        }
    }
}