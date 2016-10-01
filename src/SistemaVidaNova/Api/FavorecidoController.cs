using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;
using SistemaVidaNova.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize]    
    [Route("api/[controller]")]
    public class FavorecidoController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;
        public FavorecidoController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<FavorecidoController>();
            _environment = environment;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<FavorecidoDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            
            IQueryable<Favorecido> query = _context.Favorecido                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<FavorecidoDTO> favorecidos = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new FavorecidoDTO
                {
                    Id = v.CodFavorecido,                    
                    Nome = v.Nome,
                    Apelido = v.Apelido,
                    Cpf = v.Cpf,
                    Rg = v.Rg,                    
                    Sexo = v.Sexo,
                    DataNascimento = v.DataNascimento,                    
                    DataDeCadastro = v.DataDeCadastro
                }).ToList();

            
            return favorecidos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Favorecido f =_context.Favorecido                
                .SingleOrDefault(q => q.CodFavorecido == id);

            if (f == null)
                return new NotFoundResult();

            _context.ConhecimentoProficional.Where(q => q.CodFavorecido == id).Load();

            FavorecidoDTO dto = new FavorecidoDTO
            {
                Id = f.CodFavorecido,
                Nome = f.Nome,
                Apelido = f.Apelido,
                Cpf = f.Cpf,
                Rg = f.Rg,
                Sexo = f.Sexo,
                DataNascimento = f.DataNascimento,
                DataDeCadastro = f.DataDeCadastro,

            };
            _context.Familia.Include(q => q.Endereco).Where(q => q.CodFavorecido == id).Load();
            if (f.Familia != null)
            {

                dto.Familia = new FamiliaDTO()
                {
                    id = f.Familia.CodFamilia,
                    Celular = f.Familia.Celular,
                    Email = f.Familia.Email,
                    Nome = f.Familia.Nome,
                    Telefone = f.Familia.Telefone,
                    Endereco = new EnderecoDTO()
                    {
                        Bairro = f.Familia.Endereco.Bairro,
                        Cep = f.Familia.Endereco.Cep,
                        Cidade = f.Familia.Endereco.Cidade,
                        Complemento = f.Familia.Endereco.Complemento,
                        Estado = f.Familia.Endereco.Estado,
                        Logradouro = f.Familia.Endereco.Logradouro,
                        Numero = f.Familia.Endereco.Numero

                    }

                };

                }
            

            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FavorecidoDTO v)
        {   
            if (ModelState.IsValid)
            {
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                Favorecido favorecido = new Favorecido
                {

                    
                    Nome = v.Nome,
                    Apelido = v.Apelido,
                    Cpf = v.Cpf,
                    Rg = v.Rg,                    
                    Sexo = v.Sexo,
                    DataNascimento = v.DataNascimento,                    
                    DataDeCadastro = DateTime.Today,                    
                    IdUsuario = user.Id,                     

                };

                if (v.Familia != null)
                {
                    favorecido.Familia = new Familia()
                    {
                        Nome = v.Familia.Nome,
                        Celular = v.Familia.Nome,
                        Email = v.Familia.Nome,
                        Telefone = v.Familia.Nome

                    };
                    favorecido.Familia.Endereco = new Endereco()
                    {
                        Bairro = v.Familia.Endereco.Bairro,
                        Cep = v.Familia.Endereco.Cep,
                        Cidade = v.Familia.Endereco.Cidade,
                        Complemento = v.Familia.Endereco.Complemento,
                        Estado = v.Familia.Endereco.Estado,
                        Logradouro = v.Familia.Endereco.Logradouro,
                        Numero = v.Familia.Endereco.Numero

                    };
                }

                if (v.ConhecimentosProfissionais != null)
                {
                    foreach(var cp in v.ConhecimentosProfissionais)
                    {
                        favorecido.ConhecimentosProfissionais.Add(new ConhecimentoProficional() { Nome = cp.Text });
                    }
                }

                

                    _context.Favorecido.Add(favorecido);
                    try
                {
                    _context.SaveChanges();
                    v.Id = favorecido.CodFavorecido;
                    

                }
                catch
                {
                    return new BadRequestResult();
                }
                               
                return new ObjectResult(v);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public  IActionResult Put(int id, [FromBody]FavorecidoDTO voluntario)
        {
            if(id != voluntario.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Favorecido v = _context.Favorecido.Include(q=>q.Familia).SingleOrDefault(q => q.CodFavorecido == id );
                if (v == null)
                    return new BadRequestResult();

                
                v.Nome = voluntario.Nome;                
                v.Cpf = voluntario.Cpf;
                v.Rg = voluntario.Rg;                
                v.Sexo = voluntario.Sexo;
                v.DataNascimento = voluntario.DataNascimento;
                v.Apelido = voluntario.Apelido;
                
                if(voluntario.Familia!= null)
                {
                    if (v.Familia == null)
                    {
                        v.Familia = new Familia();
                        v.Familia.Endereco = new Endereco();
                    }

                    v.Familia.Nome = voluntario.Familia.Nome;
                    v.Familia.Telefone = voluntario.Familia.Telefone;
                    v.Familia.Email = voluntario.Familia.Email;
                    v.Familia.Celular = voluntario.Familia.Celular;


                    v.Familia.Endereco.Logradouro = voluntario.Familia.Endereco.Logradouro;
                    v.Familia.Endereco.Numero = voluntario.Familia.Endereco.Numero;
                    v.Familia.Endereco.Bairro = voluntario.Familia.Endereco.Bairro;
                    v.Familia.Endereco.Estado = voluntario.Familia.Endereco.Estado;
                    v.Familia.Endereco.Cep = voluntario.Familia.Endereco.Cep;
                    v.Familia.Endereco.Cidade = voluntario.Familia.Endereco.Cidade;
                    v.Familia.Endereco.Complemento = voluntario.Familia.Endereco.Complemento;
                }

                if (v.ConhecimentosProfissionais == null)
                    v.ConhecimentosProfissionais = new List<ConhecimentoProficional>();

                if (voluntario.ConhecimentosProfissionais == null)
                    voluntario.ConhecimentosProfissionais = new List<ConhecimentoProficionalDTO>();

                var entraram = voluntario.ConhecimentosProfissionais.Where(q => !v.ConhecimentosProfissionais.Any(x => x.Nome == q.Text));
                var sairam = v.ConhecimentosProfissionais.Where(q => !voluntario.ConhecimentosProfissionais.Any(x => x.Text == q.Nome));

                foreach(var entrou in entraram)
                {
                    v.ConhecimentosProfissionais.Add(new ConhecimentoProficional() { Nome = entrou.Text });
                }
                foreach(var saiu in sairam)
                {
                    v.ConhecimentosProfissionais.Remove(saiu);
                }






                _context.SaveChanges();


                
                

                return new ObjectResult(voluntario);
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
            Favorecido favorecido = _context.Favorecido.Include(q=>q.Familia).Single(q => q.CodFavorecido == id);
            if(favorecido.Familia!=null)
            {
                Endereco end = _context.Endereco.Single(q => q.Id == favorecido.Familia.IdEndereco);
                _context.Endereco.Remove(end);
            }
            _context.Favorecido.Remove(favorecido);
            
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

        
        [HttpGet("pdf/{id}")]
        public IActionResult GetFile(string id)
        {
            var fileName = $"export.pfd";
            var filepath = $"Downloads/{fileName}";
            byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
            return File(fileBytes, "application/pdf", fileName);
        }
    }
}
