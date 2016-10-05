using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class EventoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        public EventoController(VidaNovaContext context,UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<EventoDTO> Get([FromQuery]DateTime? start, [FromQuery]DateTime? end, [FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            IQueryable<Evento> query = _context.Evento.Include(q=>q.Usuario);
             if(start!=null && end != null)//busca pelo calendario
            {
                query = query.Where(q => q.DataInicio >= start && q.DataInicio <= end);
            }
            else { // busca pela lista
                if (skip == null)
                    skip = 0;
                if (take == null)
                    take = 1000;

                 query = query
                    .OrderByDescending(q => q.DataInicio);

                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.Titulo.Contains(filtro));

                this.Response.Headers.Add("totalItems", query.Count().ToString());
                query = query
                .Skip(skip.Value)
                .Take(take.Value);
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
                                                     
                                                     valorArrecadado = q.ValorArrecadado,
                                                     relato = q.Relato,
                                                       Usuario = new UsuarioDTO()
                                                       {
                                                            Id = q.Usuario.Id,
                                                             Email = q.Usuario.Email,
                                                              Nome = q.Usuario.Nome
                                                       }                                                   
                                                       
                                                     
                                                 }).ToList();
              return eventos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Evento eve = _context.Evento
                    .Include(q => q.Interessados)
                    //.ThenInclude(q=>q.Interessado)
                    .Include(q => q.Voluntarios)
                    .Include(q=>q.Usuario)
                    //.ThenInclude(q=> q.Voluntario)
                    .SingleOrDefault(q => q.CodEvento == id);
            if (eve == null)
                return new NotFoundResult();
            if (eve.Interessados.Count > 0)
                _context.InteressadoEvento.Where(q => q.CodEvento == id).Include(q => q.Interessado).Load();
            if (eve.Voluntarios.Count > 0)
                _context.VoluntarioEvento.Where(q => q.CodEvento == id).Include(q => q.Voluntario).Load();
            EventoDTO dto = new EventoDTO
            {
                id = eve.CodEvento,
                title = eve.Titulo,
                descricao = eve.Descricao,
                color = eve.Cor,
                textColor = eve.CorDaFonte,
                start = eve.DataInicio,
                end = eve.DataFim,
                
                valorArrecadado = eve.ValorArrecadado,
                relato = eve.Relato,
                 
                Usuario = new UsuarioDTO()
                {
                    Id = eve.Usuario.Id,
                    Email = eve.Usuario.Email,
                    Nome = eve.Usuario.Nome
                },

                voluntarios = eve.Voluntarios.Select(q=>new VoluntarioDTO{
                 Id = q.IdVoluntario,
                 Nome = q.Voluntario.Nome,
                 Cpf = q.Voluntario.Cpf}).ToList(),
                interessados = eve.Interessados.Select(q => new InteressadoDTO
                {
                    Id = q.Interessado.CodInteressado,
                    Nome = q.Interessado.Nome
                }).ToList()

            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]EventoDTO evento)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);
                Evento novo = new Evento()
                {

                    Titulo = evento.title,
                Descricao = evento.descricao,
                Cor = evento.color,
                CorDaFonte = evento.textColor,
                DataInicio = evento.start,
                DataFim = evento.end,
                
                ValorArrecadado = evento.valorArrecadado,
                Relato = evento.relato,
                 Usuario=usuario
                 
                 
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

                

                Evento e = _context.Evento
                    .Include(q=>q.Interessados)
                    .Include(q=>q.Voluntarios).
                    Single(q => q.CodEvento == id);

                e.Titulo = evento.title;
                e.Descricao = evento.descricao;
                e.Cor = evento.color;
                e.CorDaFonte = evento.textColor;
                e.DataInicio = evento.start;
                e.DataFim = evento.end;
                
                e.ValorArrecadado = evento.valorArrecadado;
                e.Relato = evento.relato;



                
                if (evento.interessados != null)
                {
                    //verifica quem está presentee e quem saiu
                    List<InteressadoEvento> corretos = new List<InteressadoEvento>();
                    foreach (var inter in evento.interessados)
                    {
                        var eventoInteressado = e.Interessados.SingleOrDefault(q => q.CodInteressado == inter.Id);
                        if (eventoInteressado == null)
                        {
                            eventoInteressado = new InteressadoEvento { CodEvento = e.CodEvento, CodInteressado = inter.Id };
                            corretos.Add(eventoInteressado);
                            e.Interessados.Add(eventoInteressado);


                        }
                        corretos.Add(eventoInteressado);
                        
                    }
                    //remove incorretos
                    var incorretos = e.Interessados.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Interessados.Remove(incorreto);

                }
                else
                    evento.interessados.Clear();

                
                if (evento.voluntarios != null)
                {
                    //verifica quem está presentee e quem saiu
                    List<VoluntarioEvento> corretos = new List<VoluntarioEvento>();
                    foreach (var volunt in evento.voluntarios)
                    {
                        var eventoVoluntario = e.Voluntarios.SingleOrDefault(q => q.IdVoluntario == volunt.Id);
                        if (eventoVoluntario == null)
                        {
                            eventoVoluntario = new VoluntarioEvento { CodEvento = e.CodEvento, IdVoluntario = volunt.Id };
                            corretos.Add(eventoVoluntario);
                            e.Voluntarios.Add(eventoVoluntario);

                        }
                        corretos.Add(eventoVoluntario);

                    }
                    //remove incorretos
                    var incorretos = e.Voluntarios.Except(corretos);
                    foreach (var incorreto in incorretos.ToArray())
                        e.Voluntarios.Remove(incorreto);
                }
                else
                    evento.voluntarios.Clear();

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
