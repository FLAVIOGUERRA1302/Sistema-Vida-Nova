using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class DoacaoDinheiroController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public DoacaoDinheiroController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DoacaoDinheiroDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<DoacaoDinheiro> query = _context.DoacaoDinheiro.Include(q=>q.Doador)                
                .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<DoacaoDinheiroDTO> doacoes = new List<DoacaoDinheiroDTO>();
            query = query.Skip(skip.Value).Take(take.Value);
            foreach( var v in query)
            {
                doacoes.Add(new DoacaoDinheiroDTO
                {
                    Id = v.Id,
                    DataDaDoacao = v.Data,
                    Descricao = v.Descricao,
                    Doador = new DoadorDTOR()
                    {
                        Id = v.Doador.CodDoador,
                        NomeRazaoSocial = v.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)v.Doador).Nome : ((PessoaJuridica)v.Doador).RazaoSocial
                    },
                    Valor = v.Valor
                });
            }
           

              return doacoes;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DoacaoDinheiro q = _context.DoacaoDinheiro.Include(d=>d.Doador).SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            DoacaoDinheiroDTO dto = new DoacaoDinheiroDTO
            {
                Id = q.Id,
                DataDaDoacao = q.Data,
                Descricao = q.Descricao,
                Doador = new DoadorDTOR()
                {
                    Id = q.Doador.CodDoador,
                    NomeRazaoSocial = q.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)q.Doador).Nome : ((PessoaJuridica)q.Doador).RazaoSocial,
                    Tipo = q.Doador.GetType() == typeof(PessoaFisica)?"PF":"PJ"

                },
                Valor = q.Valor
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]DoacaoDinheiroDTO dto)
        {
            if (ModelState.IsValid)
            {
                
                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if(doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoDinheiro novo = new DoacaoDinheiro()
                {
                    Doador = doador,
                    Data = dto.DataDaDoacao,
                    Descricao = dto.Descricao,
                    Valor = dto.Valor
                };
                try
                {
                    _context.DoacaoDinheiro.Add(novo);
                
                    _context.SaveChanges();
                    dto.Id = novo.Id;
                    return new ObjectResult(dto);
                }
                catch {
                    
                    return new BadRequestObjectResult(ModelState);
                }
                


            }
            
                return new BadRequestObjectResult(ModelState);
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DoacaoDinheiroDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoDinheiro dd = _context.DoacaoDinheiro.Single(q => q.Id == id);

                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if (doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }

                dd.Doador = doador;
                dd.Data = dto.DataDaDoacao;
                dd.Descricao = dto.Descricao;
                dd.Valor = dto.Valor;


                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    
                    return new BadRequestObjectResult(ModelState);
                }

                return new ObjectResult(dto);
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
            DoacaoDinheiro dd = _context.DoacaoDinheiro.Single(q => q.Id == id);
            _context.DoacaoDinheiro.Remove(dd);
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
