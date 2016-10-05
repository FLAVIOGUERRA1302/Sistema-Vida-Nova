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
    public class InteressadoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public InteressadoController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<InteressadoDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<Interessado> query = _context.Interessado                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<InteressadoDTO> interessados = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new InteressadoDTO
                {
                    Id = v.CodInteressado,
                    Email = v.Email,
                    Nome = v.Nome,
                    Celular = v.Celular,
                    Telefone = v.Telefone
                }).ToList();

              return interessados;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Interessado q = _context.Interessado.SingleOrDefault(i => i.CodInteressado == id);
            if (q == null)
                return new NotFoundResult();

            InteressadoDTO dto = new InteressadoDTO
            {
                Id = q.CodInteressado,
                Celular = q.Celular,
                Email = q.Email,
                Nome = q.Nome,
                Telefone = q.Telefone/*,
                Eventos = q.Eventos.OrderByDescending(e => e.DataInicio)
                    .Select(e => new EventoDTO
                    {
                        id = e.CodEvento,
                        descricao = e.Descricao,
                        title = e.Titulo,
                        start = e.DataInicio,
                        end = e.DataFim,
                        valorDeEntrada = e.ValorDeEntrada,
                        valorArrecadado = e.ValorArrecadado
                    }).ToList()*/
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]InteressadoDTO i)
        {
            if (ModelState.IsValid)
            {
                Interessado novo = new Interessado()
                {
                    Nome = i.Nome,
                    Celular = i.Celular,
                    Email = i.Email,
                    Telefone = i.Telefone
                };
                _context.Interessado.Add(novo);
                try
                {
                    _context.SaveChanges();
                    i.Id = novo.CodInteressado;
                    return new ObjectResult(i);
                }
                catch {
                    ModelState.AddModelError("Email", "Este email já está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]InteressadoDTO interessado)
        {
            if (id != interessado.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                Interessado i = _context.Interessado.Single(q => q.CodInteressado == id);


                i.Nome = interessado.Nome;
                i.Celular = interessado.Celular;
                i.Telefone = interessado.Telefone;
                i.Email = interessado.Email;
                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("Email", "Este email já está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }

                return new ObjectResult(interessado);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Interessado interessado = _context.Interessado.Single(q => q.CodInteressado == id);
            _context.Interessado.Remove(interessado);
            try
            {
                _context.SaveChanges();
                return new NoContentResult();
            }
            catch
            {
                return new BadRequestResult();
            }
        }
    }
}
