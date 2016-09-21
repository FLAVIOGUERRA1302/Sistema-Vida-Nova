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
    [AllowAnonymous]
    public class EventoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public EventoController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<EventoDTO> Get([FromQuery]DateTime? start, [FromQuery]DateTime? end)
        {

            IQueryable<Evento> query = null;
             if(start!=null && end != null)
            {
                query = _context.Evento.Where(q => q.DataInicio >= start && q.DataInicio <= end);
            }
            else
            {
                query = _context.Evento;
            }

            List<EventoDTO> eventos = (from q in query
                                       select new EventoDTO
                                                 {
                                                     id = q.CodEvento,
                                                     title = q.Titulo,
                                                     descricao = q.Descricao,
                                                     color = q.Cor,
                                                     textColor = q.CorDaFonte,
                                                     start = q.DataInicio,
                                                     end = q.DataFim,
                                                     valorDeEntrada = q.ValorDeEntrada,
                                                     valorArrecadado = q.ValorArrecadado,
                                                     relato = q.Relato

                                                 }).ToList();
              return eventos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Evento q = _context.Evento.SingleOrDefault(i => i.CodEvento == id);
            if (q == null)
                return new NotFoundResult();

            EventoDTO dto = new EventoDTO
            {
                id = q.CodEvento,
                title = q.Titulo,
                descricao = q.Descricao,
                color = q.Cor,
                textColor = q.CorDaFonte,
                start = q.DataInicio,
                end = q.DataFim,
                valorDeEntrada = q.ValorDeEntrada,
                valorArrecadado = q.ValorArrecadado,
                relato = q.Relato

            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]EventoDTO evento)
        {
            if (ModelState.IsValid)
            {
                Evento novo = new Evento()
                {

                    Titulo = evento.title,
                Descricao = evento.descricao,
                Cor = evento.color,
                CorDaFonte = evento.textColor,
                DataInicio = evento.start,
                DataFim = evento.end,
                ValorDeEntrada = evento.valorDeEntrada,
                ValorArrecadado = evento.valorArrecadado,
                Relato = evento.relato,
                 VoluntarioId = evento.Voluntario.Id
            };
                _context.Evento.Add(novo);
                try
                {
                    _context.SaveChanges();
                    
                    return new ObjectResult(evento);
                }
                catch {

                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EventoDTO evento)
        {
            if (id != evento.id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                Evento i = _context.Evento.Single(q => q.CodEvento == id);

                i.Titulo = evento.title;
                i.Descricao = evento.descricao;
                i.Cor = evento.color;
                i.CorDaFonte = evento.textColor;
                i.DataInicio = evento.start;
                i.DataFim = evento.end;
                i.ValorDeEntrada = evento.valorDeEntrada;
                i.ValorArrecadado = evento.valorArrecadado;
                i.Relato = evento.relato;

                _context.SaveChanges();

                return new ObjectResult(evento);
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
            Evento evento = _context.Evento.Single(q => q.CodEvento == id);
            _context.Evento.Remove(evento);
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
