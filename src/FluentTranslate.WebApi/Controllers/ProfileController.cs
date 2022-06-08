using Microsoft.AspNetCore.Mvc;

namespace FluentTranslate.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProfileController> _logger;
        protected ILogger<ProfileController> Logger
        {
            get => _logger;
        }

        public ProfileController(ILogger<ProfileController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetProfileList")]
        public IEnumerable<string> GetProfiles()
        {
            return Summaries;
        }

        [HttpGet(Name = "GetProfile")]
        public IEnumerable<string> GetProfile(string name)
        {
            return Summaries;
        }
    }
}