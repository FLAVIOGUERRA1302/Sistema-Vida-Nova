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
    public class DoacaoObjetoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        public DoacaoObjetoController(VidaNovaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<DoacaoObjetoDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<DoacaoObjeto> query = _context.DoacaoObjeto.Include(q=>q.Doador)                
                .OrderByDescending(q => q.DataDaDoacao);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<DoacaoObjetoDTO> doacoes = new List<DoacaoObjetoDTO>();
            query = query.Skip(skip.Value).Take(take.Value);
            foreach( var v in query)
            {
                doacoes.Add(new DoacaoObjetoDTO
                {
                    Id = v.Id,
                    DataDaDoacao = v.DataDaDoacao,
                    Descricao = v.Descricao,
                    Doador = new DoadorDTOR()
                    {
                        Id = v.Doador.CodDoador,
                        NomeRazaoSocial = v.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)v.Doador).Nome : ((PessoaJuridica)v.Doador).RazaoSocial
                    },
                     DataDeRetirada = v.DataDeRetirada
                });
            }
           

              return doacoes;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            DoacaoObjeto q = _context.DoacaoObjeto.Include(d=>d.Doador).Include(d=>d.Endereco).SingleOrDefault(i => i.Id == id);
            if (q == null)
                return new NotFoundResult();

            DoacaoObjetoDTO dto = new DoacaoObjetoDTO
            {
                Id = q.Id,
                DataDaDoacao = q.DataDaDoacao,
                Descricao = q.Descricao,
                Doador = new DoadorDTOR()
                {
                    Id = q.Doador.CodDoador,
                    NomeRazaoSocial = q.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)q.Doador).Nome : ((PessoaJuridica)q.Doador).RazaoSocial,
                    Tipo = q.Doador.GetType() == typeof(PessoaFisica)?"PF":"PJ"

                },
                DataDeRetirada=q.DataDeRetirada,
                Endereco = new EnderecoDTO()
                {
                     Id = q.Endereco.Id,
                     Bairro = q.Endereco.Bairro,
                     Cep = q.Endereco.Cep,
                     Cidade = q.Endereco.Cidade,
                     Complemento = q.Endereco.Complemento,
                     Estado = q.Endereco.Estado,
                     Logradouro = q.Endereco.Logradouro,
                     Numero = q.Endereco.Numero,
                }
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]DoacaoObjetoDTO dto)
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
                DoacaoObjeto novo = new DoacaoObjeto()
                {
                    Doador = doador,
                    DataDaDoacao = dto.DataDaDoacao,
                    Descricao = dto.Descricao,
                    DataDeRetirada = dto.DataDeRetirada,
                    Endereco = new Endereco
                    {
                        Bairro = dto.Endereco.Bairro,
                        Cep = dto.Endereco.Cep,
                        Cidade = dto.Endereco.Cidade,
                        Complemento = dto.Endereco.Complemento,
                        Estado = dto.Endereco.Estado,
                        Logradouro = dto.Endereco.Logradouro,
                        Numero = dto.Endereco.Numero
                    }
                };
                try
                {
                    _context.DoacaoObjeto.Add(novo);
                
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
        public IActionResult Put(int id, [FromBody]DoacaoObjetoDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.DataDaDoacao = dto.DataDaDoacao.AddHours(-dto.DataDaDoacao.Hour);
                DoacaoObjeto dd = _context.DoacaoObjeto.Include(q=>q.Endereco).Single(q => q.Id == id);

                Doador doador = _context.Doador.SingleOrDefault(q => q.CodDoador == dto.Doador.Id);
                if (doador == null)
                {
                    ModelState.AddModelError("Doador", "Doador inválido");
                    return new BadRequestObjectResult(ModelState);
                }

                dd.Doador = doador;
                dd.DataDaDoacao = dto.DataDaDoacao;
                dd.Descricao = dto.Descricao;
                dd.DataDeRetirada = dto.DataDeRetirada;

                dd.Endereco.Cep = dto.Endereco.Cep;
                dd.Endereco.Bairro = dto.Endereco.Bairro;
                dd.Endereco.Cidade = dto.Endereco.Cidade;
                dd.Endereco.Complemento = dto.Endereco.Complemento;
                dd.Endereco.Estado = dto.Endereco.Estado;
                dd.Endereco.Numero = dto.Endereco.Numero;
                dd.Endereco.Logradouro = dto.Endereco.Logradouro;
                


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
            DoacaoObjeto dd = _context.DoacaoObjeto.Single(q => q.Id == id);
            _context.DoacaoObjeto.Remove(dd);
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
