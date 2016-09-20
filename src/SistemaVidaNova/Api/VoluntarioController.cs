using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    public class VoluntarioController : Controller
    {
        private VidaNovaContext _context;
        public VoluntarioController(VidaNovaContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Voluntario> Get()
        {
            List<Voluntario> voluntarios = _context.Voluntario.ToList();

            return voluntarios;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Voluntario v =_context.Voluntario.SingleOrDefault(q => q.Id == id);
            if (v == null)
                return new NotFoundResult();

            return new ObjectResult(v);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Voluntario voluntario)
        {
            if (ModelState.IsValid)
            {
                _context.Voluntario.Add(voluntario);
                try
                {
                    _context.SaveChanges();
                    return new ObjectResult(voluntario);
                }
                catch
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
            }else
            {
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Voluntario voluntario)
        {
            if(id != voluntario.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);

            _context.Voluntario.Update(voluntario);
            _context.SaveChanges();
            return new ObjectResult(voluntario);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Voluntario voluntario = _context.Voluntario.Single(q => q.Id == id);
            _context.Voluntario.Remove(voluntario);
            try
            {
                _context.SaveChanges();
                return new StatusCodeResult(200);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
        }
    }
}
