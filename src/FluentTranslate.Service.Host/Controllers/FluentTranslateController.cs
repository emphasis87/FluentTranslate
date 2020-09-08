using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FluentTranslate.Service.Host.Controllers
{
    [Route("api/translate")]
    [ApiController]
    public class FluentTranslateController : ControllerBase
    {
        // GET: api/<FluentTranslateController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FluentTranslateController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FluentTranslateController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FluentTranslateController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FluentTranslateController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
