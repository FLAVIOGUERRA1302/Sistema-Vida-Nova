using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;
using SistemaVidaNova.Models.DTOs;
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
    public class VoluntarioController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;
        public VoluntarioController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<VoluntarioController>();
            _environment = environment;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<VoluntarioDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            
            IQueryable<Voluntario> query = _context.Voluntario
                .Where(q => q.IsDeletado == false)
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            this.Response.Headers.Add("totalItems", query.Count().ToString());

            List<VoluntarioDTO> voluntarios = query
                .Skip(skip.Value)
                .Take(take.Value).Select(v => new VoluntarioDTO
                {
                    Id = v.Id,
                    Email = v.Email,
                    Nome = v.Nome,
                    Cpf = v.Cpf,
                    Rg = v.Rg,
                    Celular = v.Celular,
                    Telefone = v.Telefone,
                    Sexo = v.Sexo,
                    Funcao = v.Funcao,
                    DataNascimento = v.DataNascimento,
                    SegundaFeira = v.SegundaFeira,
                    TercaFeira = v.TercaFeira,
                    QuartaFeira = v.QuartaFeira,
                    QuintaFeira = v.QuintaFeira,
                    SextaFeira = v.SextaFeira,
                    Sabado = v.Sabado,
                    Domingo = v.Domingo,
                    DataDeCadastro = v.DataDeCadastro
                }).ToList();

            
            return voluntarios;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Voluntario v =_context.Voluntario
                .Include(q=>q.Endereco)
                .SingleOrDefault(q => q.Id == id);

            if (v == null)
                return new NotFoundResult();

            
            VoluntarioDTO dto = new VoluntarioDTO
            {
                Id = v.Id,
                Email = v.Email,
                Nome = v.Nome,
                Cpf = v.Cpf,
                Rg = v.Rg,
                Celular = v.Celular,
                Telefone = v.Telefone,
                Funcao = v.Funcao,
                Sexo = v.Sexo,
                DataNascimento = v.DataNascimento,
                SegundaFeira = v.SegundaFeira,
                TercaFeira = v.TercaFeira,
                QuartaFeira = v.QuartaFeira,
                QuintaFeira = v.QuintaFeira,
                SextaFeira = v.SextaFeira,
                Sabado = v.Sabado,
                Domingo = v.Domingo,
                DataDeCadastro = v.DataDeCadastro,
                Endereco = new EnderecoDTO()
                {
                     Id = v.Endereco.Id,
                    Bairro = v.Endereco.Bairro,
                    Cep = v.Endereco.Cep,
                    Cidade = v.Endereco.Cidade,
                    Complemento = v.Endereco.Complemento,
                    Estado = v.Endereco.Estado,
                    Logradouro = v.Endereco.Logradouro,
                    Numero = v.Endereco.Numero

                }

        };
            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VoluntarioDTO v)
        {   
            if (ModelState.IsValid)
            {
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                Voluntario voluntario = _context.Voluntario.Include(q=>q.Endereco).SingleOrDefault(q => q.Cpf == v.Cpf && q.IsDeletado==true);
                if (voluntario == null)
                {
                    voluntario = new Voluntario
                    {

                        Email = v.Email,
                        Nome = v.Nome,
                        Cpf = v.Cpf,
                        Rg = v.Rg,
                        Celular = v.Celular,
                        Telefone = v.Telefone,
                        Sexo = v.Sexo,
                        Funcao = v.Funcao,
                        DataNascimento = v.DataNascimento,
                        SegundaFeira = v.SegundaFeira,
                        TercaFeira = v.TercaFeira,
                        QuartaFeira = v.QuartaFeira,
                        QuintaFeira = v.QuintaFeira,
                        SextaFeira = v.SextaFeira,
                        Sabado = v.Sabado,
                        Domingo = v.Domingo,
                        DataDeCadastro = DateTime.Today,
                        IsDeletado = false,
                        IdUsuario = user.Id
                    };

                    voluntario.Endereco = new Endereco()
                    {
                        Bairro = v.Endereco.Bairro,
                        Cep = v.Endereco.Cep,
                        Cidade = v.Endereco.Cidade,
                        Complemento = v.Endereco.Complemento,
                        Estado = v.Endereco.Estado,
                        Logradouro = v.Endereco.Logradouro,
                        Numero = v.Endereco.Numero

                    };
                }else
                {
                    voluntario.Email = v.Email;
                    voluntario.Nome = v.Nome;
                    voluntario.Cpf = v.Cpf;
                    voluntario.Rg = v.Rg;
                    voluntario.Celular = v.Celular;
                    voluntario.Telefone = v.Telefone;
                    voluntario.Sexo = v.Sexo;
                    voluntario.Funcao = v.Funcao;
                    voluntario.DataNascimento = v.DataNascimento;
                    voluntario.SegundaFeira = v.SegundaFeira;
                    voluntario.TercaFeira = v.TercaFeira;
                    voluntario.QuartaFeira = v.QuartaFeira;
                    voluntario.QuintaFeira = v.QuintaFeira;
                    voluntario.SextaFeira = v.SextaFeira;
                    voluntario.Sabado = v.Sabado;
                    voluntario.Domingo = v.Domingo;
                    voluntario.DataDeCadastro = DateTime.Today;
                    voluntario.IsDeletado = false;
                    voluntario.IdUsuario = user.Id;

                    if(voluntario.Endereco != null)
                    {
                        _context.Remove(voluntario.Endereco);
                    }

                    voluntario.Endereco = new Endereco()
                    {
                        Bairro = v.Endereco.Bairro,
                        Cep = v.Endereco.Cep,
                        Cidade = v.Endereco.Cidade,
                        Complemento = v.Endereco.Complemento,
                        Estado = v.Endereco.Estado,
                        Logradouro = v.Endereco.Logradouro,
                        Numero = v.Endereco.Numero

                    };

                }
                    _context.Voluntario.Add(voluntario);
                    try
                {
                    _context.SaveChanges();
                    v.Id = voluntario.Id;
                    var path = Path.Combine(_environment.WebRootPath, "images\\voluntarios\\");
                    System.IO.File.Copy(path+"default.jpg", path+ voluntario.Id+".jpg", true);

                }
                catch(Exception e)
                {
                        if(e.InnerException.Message.Contains("Email"))
                        ModelState.AddModelError("Email", "Este email ja está cadastrado");
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
        public  IActionResult Put(int id, [FromBody]VoluntarioDTO voluntario)
        {
            if(id != voluntario.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Voluntario v = _context.Voluntario.Include(q=>q.Endereco).SingleOrDefault(q => q.Id == id && q.IsDeletado==false);
                if (v == null)
                    return new BadRequestResult();

                v.Email = voluntario.Email;
                v.Nome = voluntario.Nome;                
                v.Cpf = voluntario.Cpf;
                v.Rg = voluntario.Rg;
                v.Celular = voluntario.Celular;
                v.Telefone = voluntario.Telefone;
                v.Funcao = voluntario.Funcao;
                v.Sexo = voluntario.Sexo;
                v.DataNascimento = voluntario.DataNascimento;
                v.SegundaFeira = voluntario.SegundaFeira;
                v.TercaFeira = voluntario.TercaFeira;
                v.QuartaFeira = voluntario.QuartaFeira;
                v.QuintaFeira = voluntario.QuintaFeira;
                v.SextaFeira = voluntario.SextaFeira;
                v.Sabado = voluntario.Sabado;
                v.Domingo = voluntario.Domingo;

                v.Endereco.Logradouro = voluntario.Endereco.Logradouro;
                v.Endereco.Numero = voluntario.Endereco.Numero;
                v.Endereco.Bairro = voluntario.Endereco.Bairro;
                v.Endereco.Estado = voluntario.Endereco.Estado;
                v.Endereco.Cep = voluntario.Endereco.Cep;
                v.Endereco.Cidade = voluntario.Endereco.Cidade;
                v.Endereco.Complemento = voluntario.Endereco.Complemento;



                try
                {
                    _context.SaveChanges();
                }                
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Email"))
                        ModelState.AddModelError("Email", "Este email ja está cadastrado");
                    if (e.InnerException.Message.Contains("Cpf"))
                        ModelState.AddModelError("Cpf", "Este CPF ja está cadastrado");
                    return new BadRequestObjectResult(ModelState);
                }
            

                
                

                return new ObjectResult(voluntario);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id)
        {
            var uploads = Path.Combine(_environment.WebRootPath, "images\\voluntarios");
            var file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, id+".jpg"), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
               
             
            return new OkResult();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Voluntario voluntario = _context.Voluntario.Single(q => q.Id == id);
            voluntario.IsDeletado = true;
            voluntario.Email = "";
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
