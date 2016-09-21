using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using Microsoft.AspNetCore.Http;
using SistemaVidaNova.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    //[Authorize]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class VoluntarioController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Voluntario> _userManager;
        private readonly ILogger _logger;
        public VoluntarioController(
            VidaNovaContext context,
            UserManager<Voluntario> userManager,
            ILoggerFactory loggerFactory)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<VoluntarioController>();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<VoluntarioDTO> Get()
        {
            List<VoluntarioDTO> voluntarios = (from v in _context.Voluntario
                                              select new VoluntarioDTO
                                              {
                                                  Id = v.Id,
                                                  Email = v.Email,
                                                  Nome = v.Nome,
                                                  Cpf = v.Cpf,
                                                  Rg = v.Rg,
                                                  Celular = v.Celular,
                                                  Telefone = v.Telefone,
                                                  Sexo = v.Sexo,
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
        public IActionResult Get(string id)
        {
            Voluntario v =_context.Voluntario.SingleOrDefault(q => q.Id == id);
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
                Enderecos = v.Enderecos,
                Eventos = v.Eventos.OrderByDescending(q=>q.DataInicio)
                    .Select(q=> new EventoDTO {
                            id=q.CodEvento,
                            descricao = q.Descricao,
                            title=q.Titulo,
                            start=q.DataInicio,
                            end=q.DataFim,
                            valorDeEntrada = q.ValorDeEntrada,
                            valorArrecadado = q.ValorArrecadado
                    }).ToList()
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]VoluntarioDTO v)
        {
            if (ModelState.IsValid)
            {
                Voluntario user = new Voluntario
                {
                    
                    Email = v.Email,
                    Nome = v.Nome,
                    UserName=v.Nome,
                    Cpf = v.Cpf,
                    Rg = v.Rg,
                    Celular = v.Celular,
                    Telefone = v.Telefone,
                    Sexo = v.Sexo,
                    DataNascimento = v.DataNascimento,
                    SegundaFeira = v.SegundaFeira,
                    TercaFeira = v.TercaFeira,
                    QuartaFeira = v.QuartaFeira,
                    QuintaFeira = v.QuintaFeira,
                    SextaFeira = v.SextaFeira,
                    Sabado = v.Sabado,
                    Domingo = v.Domingo,
                    DataDeCadastro = DateTime.Today
                };
                if(v.Enderecos!= null)
                    user.Enderecos.AddRange(v.Enderecos);

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {   
                    _logger.LogInformation(3, "User created a new account without password.");
                    v.Id = user.Id;
                    return new ObjectResult(v);

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return new BadRequestObjectResult(ModelState);


            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]VoluntarioDTO voluntario)
        {
            if(id != voluntario.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Voluntario v = _context.Voluntario.Single(q => q.Id == id);

                v.Email = voluntario.Email;
                v.Nome = voluntario.Nome;
                v.UserName = voluntario.Nome;
                v.Cpf = voluntario.Cpf;
                v.Rg = voluntario.Rg;
                v.Celular = voluntario.Celular;
                v.Telefone = voluntario.Telefone;
                v.Sexo = voluntario.Sexo;
                v.DataNascimento = voluntario.DataNascimento;
                v.SegundaFeira = voluntario.SegundaFeira;
                v.TercaFeira = voluntario.TercaFeira;
                v.QuartaFeira = voluntario.QuartaFeira;
                v.QuintaFeira = voluntario.QuintaFeira;
                v.SextaFeira = voluntario.SextaFeira;
                v.Sabado = voluntario.Sabado;
                v.Domingo = voluntario.Domingo;


                //atualiza os endereços
                List<Endereco> enderecosCorretos = new List<Endereco>();
                List<Endereco> enderecosAtualizados = new List<Endereco>();
                //endereços atualizados
                if (v.Enderecos == null)
                    v.Enderecos = new List<Endereco>();
                if (voluntario.Enderecos == null)
                    voluntario.Enderecos = new List<Endereco>();
                var pairs = from a in v.Enderecos
                            join n in voluntario.Enderecos on a.Id equals n.Id
                            select new
                            {
                                antigo = a,
                                novo = n
                            };
                foreach (var p in pairs)
                {
                    p.antigo.Bairro = p.novo.Bairro;
                    p.antigo.Cep = p.novo.Cep;
                    p.antigo.Complemento = p.novo.Complemento;
                    p.antigo.Estado = p.novo.Estado;
                    p.antigo.Logradouro = p.novo.Logradouro;
                    p.antigo.Numero = p.novo.Numero;
                    p.antigo.Cidade = p.novo.Cidade;

                    enderecosCorretos.Add(p.antigo);
                    enderecosAtualizados.Add(p.novo);
                }
                //endereços excluido
                var exluidos = v.Enderecos.Except(enderecosCorretos);
                foreach (Endereco excluir in exluidos)
                {
                    v.Enderecos.Remove(excluir);
                    _context.Remove(excluir);
                }

                //endereços novos
                var novos = voluntario.Enderecos.Except(enderecosAtualizados);
                foreach (Endereco end in voluntario.Enderecos)
                {
                    v.Enderecos.Add(end);
                }
                               

                _context.SaveChanges();
                voluntario.Enderecos = v.Enderecos; // para poder enviar os IDs novos
                return new ObjectResult(voluntario);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Voluntario voluntario = _context.Voluntario.Single(q => q.Id == id);
            _context.Voluntario.Remove(voluntario);
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
