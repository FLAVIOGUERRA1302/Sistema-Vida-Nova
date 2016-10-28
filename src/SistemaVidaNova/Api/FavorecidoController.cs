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
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using CustomExtensions;


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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<Favorecido> query = _context.Favorecido
                .Include(q=>q.ConhecimentosProfissionais)
                .Include(q=>q.Usuario)
                .Include(q => q.Familia)
                .ThenInclude(q => q.Endereco)
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (SaveOption == null)
                SaveOption = "ExcelXlsx";




            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Name = "Favorecidos";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 30;


            sheet.Range[1, 1, 1, 21].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Favorecidos";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Nome";
            sheet.Range[3, 3].Text = "Apelido";
            sheet.Range[3, 4].Text = "CPF";
            sheet.Range[3, 5].Text = "RG";
            sheet.Range[3, 6].Text = "Sexo";
            sheet.Range[3, 7].Text = "Dt. nascimento";
            sheet.Range[3, 8].Text = "Conhecimentos profissionais";
            sheet.Range[3, 9].Text = "Nome do familiar";
            sheet.Range[3, 10].Text = "Celular";
            sheet.Range[3, 11].Text = "Telefone";
            sheet.Range[3, 12].Text = "Email";
            sheet.Range[3, 13].Text = "Cep";
            sheet.Range[3, 14].Text = "Logradouro";
            sheet.Range[3, 15].Text = "Número";
            sheet.Range[3, 16].Text = "Complemento";
            sheet.Range[3, 17].Text = "Bairro";
            sheet.Range[3, 18].Text = "Cidade";
            sheet.Range[3, 19].Text = "Estado";            
            sheet.Range[3, 20].Text = "Dt. Cadastro";
            sheet.Range[3, 21].Text = "Usuário";


            IStyle style = sheet[3, 1, 3, 21].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var q in query)
            {
                

                sheet.Range[linha, 1].Number = q.CodFavorecido;
                sheet.Range[linha, 2].Text = q.Nome;
                sheet.Range[linha, 3].Text = q.Apelido;
                sheet.Range[linha, 4].Text = q.Cpf.ToCpf();
                sheet.Range[linha, 5].Text = q.Rg;
                sheet.Range[linha, 6].Text = q.Sexo;
                sheet.Range[linha, 7].NumberFormat = "dd/mm/yyyy";
                if (q.DataNascimento!=null)
                    sheet.Range[linha, 7].DateTime = q.DataNascimento.Value;
                if (q.ConhecimentosProfissionais.Count>0)
                    sheet.Range[linha, 8].Text = string.Join(",", q.ConhecimentosProfissionais.Select(c=>c.Nome).ToList());
                sheet.Range[linha, 9].Text = q.Familia.Nome;
                sheet.Range[linha, 10].Text = q.Familia.Celular.ToTelefone();
                sheet.Range[linha, 11].Text = q.Familia.Telefone.ToTelefone();
                sheet.Range[linha, 12].Text = q.Familia.Email;                
                sheet.Range[linha, 13].Text = q.Familia.Endereco.Cep;
                sheet.Range[linha, 14].Text = q.Familia.Endereco.Logradouro;
                sheet.Range[linha, 15].Text = q.Familia.Endereco.Numero;
                sheet.Range[linha, 16].Text = q.Familia.Endereco.Complemento;
                sheet.Range[linha, 17].Text = q.Familia.Endereco.Bairro;
                sheet.Range[linha, 18].Text = q.Familia.Endereco.Cidade;
                sheet.Range[linha, 19].Text = q.Familia.Endereco.Estado;
                sheet.Range[linha, 20].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 20].DateTime = q.DataDeCadastro;
                sheet.Range[linha, 21].Text = q.Usuario.Nome;

                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Favorecidos.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Favorecidos.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
