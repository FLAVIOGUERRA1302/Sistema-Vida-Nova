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
using System.Globalization;
using CustomExtensions;
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
        public IEnumerable<VoluntarioDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]bool? semCurso, [FromQuery]string diaDaSemana, [FromQuery]string funcao)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            if (semCurso == null)
                semCurso = false;


            IQueryable<Voluntario> query = _context.Voluntario
                .Where(q => q.IsDeletado == false)
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (!String.IsNullOrEmpty(funcao))
                query = query.Where(q => q.Funcao.Contains(funcao));


            if (!String.IsNullOrEmpty(diaDaSemana))
            {
                switch (diaDaSemana.ToUpper())
                {
                    case "DOMINGO":
                        query = query.Where(q => q.Domingo == true);
                        break;
                    case "SEGUNDA-FEIRA":
                        query = query.Where(q => q.SegundaFeira == true);
                        break;
                    case "TERÇA-FEIRA":
                        query = query.Where(q => q.TercaFeira == true);
                        break;
                    case "QUARTA-FEIRA":
                        query = query.Where(q => q.QuartaFeira == true);
                        break;
                    case "QUINTA-FEIRA":
                        query = query.Where(q => q.QuintaFeira == true);
                        break;
                    case "SEXTA-FEIRA":
                        query = query.Where(q => q.SextaFeira == true);
                        break;
                    case "SÁBADO":
                        query = query.Where(q => q.Sabado == true);
                        break;
                    
                }
            }

                

            DateTime umAnoAtras = DateTime.Today.AddYears(-1);
            if (semCurso.Value)
                query = query.Where(q => q.DataCurso <= umAnoAtras );

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
                    DataDeCadastro = v.DataDeCadastro,
                    DataCurso = v.DataCurso
                }).ToList();
            
            foreach (var dto in voluntarios)
                    dto.DiasEmAtraso = ((TimeSpan)(umAnoAtras - dto.DataCurso)).Days;
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

                },
                DataCurso = v.DataCurso,
                DiasEmAtraso = ((TimeSpan)(DateTime.Today - v.DataCurso)).Days

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
                        IdUsuario = user.Id,
                        DataCurso = DateTime.Today
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
                    voluntario.DataCurso = DateTime.Today;

                    if (voluntario.Endereco != null)
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
        public  IActionResult Put(int id, [FromBody]VoluntarioDTO dto)
        {
            if(id != dto.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Voluntario v = _context.Voluntario.Include(q=>q.Endereco).SingleOrDefault(q => q.Id == id && q.IsDeletado==false);
                if (v == null)
                    return new BadRequestResult();

                v.Email = dto.Email;
                v.Nome = dto.Nome;                
                v.Cpf = dto.Cpf;
                v.Rg = dto.Rg;
                v.Celular = dto.Celular;
                v.Telefone = dto.Telefone;
                v.Funcao = dto.Funcao;
                v.Sexo = dto.Sexo;
                v.DataNascimento = dto.DataNascimento;
                v.SegundaFeira = dto.SegundaFeira;
                v.TercaFeira = dto.TercaFeira;
                v.QuartaFeira = dto.QuartaFeira;
                v.QuintaFeira = dto.QuintaFeira;
                v.SextaFeira = dto.SextaFeira;
                v.Sabado = dto.Sabado;
                v.Domingo = dto.Domingo;

                v.Endereco.Logradouro = dto.Endereco.Logradouro;
                v.Endereco.Numero = dto.Endereco.Numero;
                v.Endereco.Bairro = dto.Endereco.Bairro;
                v.Endereco.Estado = dto.Endereco.Estado;
                v.Endereco.Cep = dto.Endereco.Cep;
                v.Endereco.Cidade = dto.Endereco.Cidade;
                v.Endereco.Complemento = dto.Endereco.Complemento;

                if (dto.DataCurso != null)
                    v.DataCurso = dto.DataCurso;
                
                

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
            

                
                

                return new ObjectResult(dto);
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


        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro, [FromQuery]string diaDaSemana)
        {

            IQueryable<Voluntario> query = _context.Voluntario
               .Where(q => q.IsDeletado == false)
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Nome.Contains(filtro));

            if (!String.IsNullOrEmpty(diaDaSemana))
            {
                switch (diaDaSemana.ToUpper())
                {
                    case "DOMINGO":
                        query = query.Where(q => q.Domingo == true);
                        break;
                    case "SEGUNDA-FEIRA":
                        query = query.Where(q => q.SegundaFeira == true);
                        break;
                    case "TERÇA-FEIRA":
                        query = query.Where(q => q.TercaFeira == true);
                        break;
                    case "QUARTA-FEIRA":
                        query = query.Where(q => q.QuartaFeira == true);
                        break;
                    case "QUINTA-FEIRA":
                        query = query.Where(q => q.QuintaFeira == true);
                        break;
                    case "SEXTA-FEIRA":
                        query = query.Where(q => q.SextaFeira == true);
                        break;
                    case "SÁBADO":
                        query = query.Where(q => q.Sabado == true);
                        break;

                }
            }

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
            sheet.Name = "Voluntários";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;
            sheet.Range[1, 7].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 15;
            sheet.Range[1, 9].ColumnWidth = 15;
            sheet.Range[1, 10].ColumnWidth = 15;
            sheet.Range[1, 11].ColumnWidth = 15;
            sheet.Range[1, 12].ColumnWidth = 15;
            sheet.Range[1, 13].ColumnWidth = 15;
            sheet.Range[1, 14].ColumnWidth = 15;
            sheet.Range[1, 15].ColumnWidth = 15;
            sheet.Range[1, 16].ColumnWidth = 15;
            sheet.Range[1, 17].ColumnWidth = 15;
            sheet.Range[1, 18].ColumnWidth = 15;
            sheet.Range[1, 19].ColumnWidth = 15;
            

            sheet.Range[1,1,1,19].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Voluntários";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Email";
            sheet.Range[3, 3].Text = "Nome";
            sheet.Range[3, 4].Text = "CPF";
            sheet.Range[3, 5].Text = "RG";
            sheet.Range[3, 6].Text = "Celular";
            sheet.Range[3, 7].Text = "Telefone";
            sheet.Range[3, 8].Text = "Sexo";
            sheet.Range[3, 9].Text = "Função";
            sheet.Range[3, 10].Text = "Dt. Nascimento";
            sheet.Range[3, 11].Text = "Segunda Feira";
            sheet.Range[3, 12].Text = "Terça Feira";
            sheet.Range[3, 13].Text = "Quarta Feira";
            sheet.Range[3, 14].Text = "Quinta Feira";
            sheet.Range[3, 15].Text = "Sexta Feira";
            sheet.Range[3, 16].Text = "Sábado";
            sheet.Range[3, 17].Text = "Domingo";
            sheet.Range[3, 18].Text = "Dt. cadastro";
            sheet.Range[3, 19].Text = "Dt. curso";            
            

            IStyle style = sheet[3,1,3,19].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach(var v in query)
            {
                sheet.Range[linha,1].Number  = v.Id;
                sheet.Range[linha, 2].Text = v.Email;
                sheet.Range[linha, 3].Text = v.Nome;

                sheet.Range[linha, 4].Text = v.Cpf.ToCpf();
                sheet.Range[linha, 5].Text = v.Rg;
                sheet.Range[linha, 6].Text =  v.Celular.ToTelefone();
                sheet.Range[linha, 7].Text =  v.Telefone.ToTelefone();
                sheet.Range[linha, 8].Text = v.Sexo;
                sheet.Range[linha, 9].Text = v.Funcao;
                sheet.Range[linha, 10].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 10].DateTime = v.DataNascimento;
                sheet.Range[linha, 11].Text = v.SegundaFeira ? "Sim" : "Não";
                sheet.Range[linha, 12].Text = v.TercaFeira ? "Sim" : "Não";
                sheet.Range[linha, 13].Text = v.QuartaFeira ? "Sim" : "Não";
                sheet.Range[linha, 14].Text = v.QuintaFeira ? "Sim" : "Não";
                sheet.Range[linha, 15].Text = v.SextaFeira ? "Sim" : "Não";
                sheet.Range[linha, 16].Text = v.Sabado ? "Sim" : "Não";
                sheet.Range[linha, 17].Text = v.Domingo ? "Sim" : "Não";
                sheet.Range[linha, 18].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 18].DateTime = v.DataDeCadastro ;
                sheet.Range[linha, 19].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 19].DateTime = v.DataCurso ;
                sheet.Range[linha, 20].NumberFormat = "dd/mm/yyyy";
                
                linha++;
            }
            
            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Voluntarios.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Voluntarios.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
