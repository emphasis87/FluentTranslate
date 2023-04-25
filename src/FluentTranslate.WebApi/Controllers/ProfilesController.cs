namespace FluentTranslate.WebApi.Controllers
{
    [ApiController]
    [Route("api/data")]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly ILogger<ProfilesController>? _logger;

        public ProfilesController(ILogger<ProfilesController>? logger = null)
        {
            _logger = logger;
        }

        [HttpGet("profile/all")]
        public IEnumerable<string> GetProfiles()
        {
            return new[] { "a", "b" };
        }

        [HttpGet("profile/{profileId}")]
        public async Task<FileStreamResult> GetProfile(string profileId)
        {
            throw new NotImplementedException();
        }
    }
}