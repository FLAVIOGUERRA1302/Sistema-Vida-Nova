using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    public class ValoresController : Controller
    {
        // GET: api/values
        private static List<string> _valores = new List<String>() { "value1", "value2" };

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _valores;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _valores[id];
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            _valores.Add(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            _valores[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _valores.RemoveAt(id);
        }
    }
}
