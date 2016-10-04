using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTO;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public UsuarioController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<UsuarioDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<Usuario> query = _context.Usuario                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro) || q.Email.Contains(filtro));

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<UsuarioDTO> interessados = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new UsuarioDTO
                {
                    Id = v.Id,
                    Email = v.Email,
                    Nome = v.Nome,
                    
                }).ToList();

              return interessados;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Usuario q = _context.Usuario.SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            UsuarioDTO dto = new UsuarioDTO
            {
                Id = q.Id,
                
                Email = q.Email,
                Nome = q.Nome
            };
            return new ObjectResult(dto);
        }

       
    }
}
