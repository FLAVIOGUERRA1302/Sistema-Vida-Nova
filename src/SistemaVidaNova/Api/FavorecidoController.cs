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
                .Include(q=>q.ConhecimentosProfissionais)
                .SingleOrDefault(q => q.CodFavorecido == id);

            if (f == null)
                return new NotFoundResult();

            //_context.ConhecimentoProficional.Where(q => q.CodFavorecido == id).Load().;
            //f.ConhecimentosProfissionais = _context.ConhecimentoProficional.Where(q => q.CodFavorecido == f.CodFavorecido).ToList();
            

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
                

                dto.Celular = f.Familia.Celular;
                dto.Email = f.Familia.Email;
                dto.NomeFamilia = f.Familia.Nome;
                dto.Telefone = f.Familia.Telefone;

                dto.Bairro = f.Familia.Endereco.Bairro;
                dto.Cep = f.Familia.Endereco.Cep;
                dto.Cidade = f.Familia.Endereco.Cidade;
                dto.Complemento = f.Familia.Endereco.Complemento;
                dto.Estado = f.Familia.Endereco.Estado;
                dto.Logradouro = f.Familia.Endereco.Logradouro;
                dto.Numero = f.Familia.Endereco.Numero;
                
              }

            dto.ConhecimentosProfissionais = f.ConhecimentosProfissionais.Select(q => new ConhecimentoProficionalDTO()
            {
                Text = q.Nome
            }).ToList();




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

                if (v.NomeFamilia != null)
                {
                    favorecido.Familia = new Familia()
                    {
                        Nome = v.NomeFamilia,
                        Celular = v.Celular,
                        Email = v.Email,
                        Telefone = v.Telefone

                    };
                    favorecido.Familia.Endereco = new Endereco()
                    {
                        Bairro = v.Bairro==null?"": v.Bairro,
                        Cep = v.Cep == null ? "" : v.Cep,
                        Cidade = v.Cidade == null ? "" : v.Cidade,
                        Complemento = v.Complemento == null ? "" : v.Complemento,
                        Estado = v.Estado == null ? "" : v.Estado,
                        Logradouro = v.Logradouro == null ? "" : v.Logradouro,
                        Numero = v.Numero == null ? "" : v.Numero

                    };
                }

                if (v.ConhecimentosProfissionais != null)
                {
                    favorecido.ConhecimentosProfissionais = new List<ConhecimentoProficional>();
                    foreach (var cp in v.ConhecimentosProfissionais)
                    {
                        ConhecimentoProficional conhecimento = new ConhecimentoProficional()
                        {
                            Favorecido = favorecido,
                            Nome = cp.Text
                        };
                        favorecido.ConhecimentosProfissionais.Add(conhecimento);
                        _context.ConhecimentoProficional.Add(conhecimento);
                    }
                }

                

                    _context.Favorecido.Add(favorecido);
                    try
                {
                    _context.SaveChanges();
                    v.Id = favorecido.CodFavorecido;
                    

                }
                catch (Exception e)
                {
                    
                    if (e.InnerException.Message.Contains("Cpf"))
                        ModelState.AddModelError("Cpf", "Este CPF ja está cadastrado");
                    return new BadRequestObjectResult(ModelState);
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
        public  IActionResult Put(int id, [FromBody]FavorecidoDTO dto)
        {
            if(id != dto.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Favorecido favorecido = _context.Favorecido.Include(q=>q.Familia).ThenInclude(q=>q.Endereco).SingleOrDefault(q => q.CodFavorecido == id );
                if (favorecido == null)
                    return new BadRequestResult();


                favorecido.Nome = dto.Nome;
                favorecido.Cpf = dto.Cpf;
                favorecido.Rg = dto.Rg;
                favorecido.Sexo = dto.Sexo;
                favorecido.DataNascimento = dto.DataNascimento;
                favorecido.Apelido = dto.Apelido;
                
                if(dto.NomeFamilia!= null)
                {
                    if (favorecido.Familia == null)
                    {
                        favorecido.Familia = new Familia();
                        favorecido.Familia.Endereco = new Endereco();
                    }


                    favorecido.Familia.Nome = dto.NomeFamilia;
                    favorecido.Familia.Celular = dto.Celular;
                    favorecido.Familia.Email = dto.Email;
                    favorecido.Familia.Telefone = dto.Telefone;

                    if (favorecido.Familia.Endereco == null)
                        favorecido.Familia.Endereco = new Endereco();

                    favorecido.Familia.Endereco.Bairro = dto.Bairro == null ? " " : dto.Bairro;
                    favorecido.Familia.Endereco.Cep = dto.Cep == null ? " " : dto.Cep;
                    favorecido.Familia.Endereco.Cidade = dto.Cidade == null ? " " : dto.Cidade;
                    favorecido.Familia.Endereco.Complemento = dto.Complemento == null ? " " : dto.Complemento;
                    favorecido.Familia.Endereco.Estado = dto.Estado == null ? " " : dto.Estado;
                    favorecido.Familia.Endereco.Logradouro = dto.Logradouro == null ? " " : dto.Logradouro;
                    favorecido.Familia.Endereco.Numero = dto.Numero == null ? " " : dto.Numero;

                    
                }

                _context.ConhecimentoProficional.Where(q => q.CodFavorecido == id).Load();

                if (favorecido.ConhecimentosProfissionais == null)
                    favorecido.ConhecimentosProfissionais = new List<ConhecimentoProficional>();

                if (dto.ConhecimentosProfissionais == null)
                    dto.ConhecimentosProfissionais = new List<ConhecimentoProficionalDTO>();

                var entraram = dto.ConhecimentosProfissionais.Where(q => !favorecido.ConhecimentosProfissionais.Any(x => x.Nome == q.Text));
                var sairam = favorecido.ConhecimentosProfissionais.Where(q => !dto.ConhecimentosProfissionais.Any(x => x.Text == q.Nome)).ToArray();

                foreach(var entrou in entraram)
                {
                    ConhecimentoProficional cp = new ConhecimentoProficional() { Nome = entrou.Text, CodFavorecido = favorecido.CodFavorecido };
                    _context.ConhecimentoProficional.Add(cp);
                    favorecido.ConhecimentosProfissionais.Add(cp);
                }
                foreach(var saiu in sairam)
                {
                    favorecido.ConhecimentosProfissionais.Remove(saiu);
                    _context.ConhecimentoProficional.Remove(saiu);
                }





                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    
                    if (e.InnerException.Message.Contains("Cpf"))
                        ModelState.AddModelError("Cpf", "Este CPF ja está cadastrado");
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
