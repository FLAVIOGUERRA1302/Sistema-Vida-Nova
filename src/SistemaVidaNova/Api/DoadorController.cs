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
    public class DoadorController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;
        public DoadorController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<DoadorController>();
            _environment = environment;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<DoadorDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]string tipo)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            if (String.IsNullOrEmpty(tipo))
                tipo = "PF"; // pessoa fisica

            List<DoadorDTO> doadores = new List<DoadorDTO>();
            if (tipo.ToUpper() == "PF")
            {
                IQueryable<PessoaFisica> query = _context.DoadorPessoaFisica
                .OrderBy(q => q.Nome);
                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.Nome.Contains(filtro) || q.Cpf.Contains(filtro));
                this.Response.Headers.Add("totalItems", query.Count().ToString());

                doadores = query
               .Skip(skip.Value)
               .Take(take.Value).Select(v => new DoadorDTO
               {
                   Id = v.CodDoador,
                   Email = v.Email,
                    NomeRazaoSocial = v.Nome,
                   Celular = v.Celular,
                   Telefone = v.Telefone,
                    CpfCnpj = v.Cpf,
                     Tipo = "PF"
               }).ToList();
            }
            else if (tipo.ToUpper() == "PJ")
            {
                IQueryable<PessoaJuridica> query = _context.DoadorPessoaJuridica
               .OrderBy(q => q.RazaoSocial);
                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.RazaoSocial.Contains(filtro) || q.Cnpj.Contains(filtro));
                this.Response.Headers.Add("totalItems", query.Count().ToString());

                doadores = query
               .Skip(skip.Value)
               .Take(take.Value).Select(v => new DoadorDTO
               {
                   Id = v.CodDoador,
                   Email = v.Email,
                   NomeRazaoSocial = v.RazaoSocial,
                   Celular = v.Celular,
                   Telefone = v.Telefone,
                   CpfCnpj = v.Cnpj,
                   Tipo = "PJ"
               }).ToList();
            }
            

            

            

            
            return doadores;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Doador d =_context.Doador
                .Include(q=>q.Endereco)
                .SingleOrDefault(q => q.CodDoador == id);

            if (d == null)
                return new NotFoundResult();

            DoadorDTO dto = new DoadorDTO();
            if(d.GetType() == typeof(PessoaFisica))
            {
                dto.Id = d.CodDoador;
                dto.NomeRazaoSocial = ((PessoaFisica)d).Nome;
                dto.CpfCnpj = ((PessoaFisica)d).Cpf;
                dto.Celular = d.Celular;
                dto.Telefone = d.Telefone;
                dto.Email = d.Email;
                dto.Tipo = "PF";
                dto.Endereco = new EnderecoDTO()
                {
                    Id = d.Endereco.Id,
                    Bairro = d.Endereco.Bairro,
                    Cep = d.Endereco.Cep,
                    Cidade = d.Endereco.Cidade,
                    Complemento = d.Endereco.Complemento,
                    Estado = d.Endereco.Estado,
                    Logradouro = d.Endereco.Logradouro,
                    Numero = d.Endereco.Numero

                };
            }
            if (d.GetType() == typeof(PessoaJuridica))
            {
                dto.Id = d.CodDoador;
                dto.NomeRazaoSocial = ((PessoaJuridica)d).RazaoSocial;
                dto.CpfCnpj = ((PessoaJuridica)d).Cnpj;
                dto.Celular = d.Celular;
                dto.Telefone = d.Telefone;
                dto.Email = d.Email;
                dto.Tipo = "PJ";
                dto.Endereco = new EnderecoDTO()
                {
                    Id = d.Endereco.Id,
                    Bairro = d.Endereco.Bairro,
                    Cep = d.Endereco.Cep,
                    Cidade = d.Endereco.Cidade,
                    Complemento = d.Endereco.Complemento,
                    Estado = d.Endereco.Estado,
                    Logradouro = d.Endereco.Logradouro,
                    Numero = d.Endereco.Numero

                };
            }

            

        
            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]DoadorDTO v)
        {   
            if (ModelState.IsValid && (v.Tipo.ToUpper()=="PJ" || v.Tipo.ToUpper() == "PF"))
            {
                

                if (v.Tipo.ToUpper() == "PF")
                {
                    //verifica se ja exite o cpf cadastrado
                    if(_context.DoadorPessoaFisica.Any(q=>q.Cpf == v.CpfCnpj))
                    {
                        ModelState.AddModelError("Cpf", "Este CPF já está cadastrado");
                        return new BadRequestObjectResult(ModelState);
                    }

                    PessoaFisica pf = new PessoaFisica()
                    {
                        Celular = v.Celular,
                        Cpf = v.CpfCnpj,
                        Email = v.Email,
                        Nome = v.NomeRazaoSocial,
                        Telefone = v.Telefone
                    };

                    pf.Endereco = new Endereco()
                    {
                        Bairro = v.Endereco.Bairro,
                        Cep = v.Endereco.Cep,
                        Cidade = v.Endereco.Cidade,
                        Complemento = v.Endereco.Complemento,
                        Estado = v.Endereco.Estado,
                        Logradouro = v.Endereco.Logradouro,
                        Numero = v.Endereco.Numero

                    };
                    
                    _context.DoadorPessoaFisica.Add(pf);
                    try
                    {
                        _context.SaveChanges();
                        v.Id = pf.CodDoador;

                    }
                    catch
                    {
                        return new BadRequestResult();
                    }
                }
                else
                {
                    if (_context.DoadorPessoaJuridica.Any(q => q.Cnpj == v.CpfCnpj))
                    {
                        ModelState.AddModelError("Cnpj", "Este CNPJ já está cadastrado");
                        return new BadRequestObjectResult(ModelState);
                    }
                    PessoaJuridica pj = new PessoaJuridica()
                    {
                        Celular = v.Celular,
                        Cnpj = v.CpfCnpj,
                        Email = v.Email,
                        RazaoSocial = v.NomeRazaoSocial,
                        Telefone = v.Telefone
                    };

                    pj.Endereco = new Endereco()
                    {
                        Bairro = v.Endereco.Bairro,
                        Cep = v.Endereco.Cep,
                        Cidade = v.Endereco.Cidade,
                        Complemento = v.Endereco.Complemento,
                        Estado = v.Endereco.Estado,
                        Logradouro = v.Endereco.Logradouro,
                        Numero = v.Endereco.Numero

                    };
                    _context.DoadorPessoaJuridica.Add(pj);
                    try
                    {
                        _context.SaveChanges();
                        v.Id = pj.CodDoador;

                    }
                    catch (Exception e)
                    {
                        if (e.InnerException.Message.Contains("Email"))
                            ModelState.AddModelError("Email", "Este email ja está cadastrado");
                        
                        return new BadRequestObjectResult(ModelState);
                    }

                }
                               
                return new ObjectResult(v);
            }
            else
            {
                if (ModelState.IsValid)
                    ModelState.AddModelError("Tipo", "Valores aceitados = ['PJ','PF']");
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public  IActionResult Put(int id, [FromBody]DoadorDTO doador)
        {
            if(id != doador.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                Doador d = _context.Doador.Include(q=>q.Endereco).SingleOrDefault(q => q.CodDoador == id );
                if (d == null)
                    return new BadRequestResult();


                

                if (d.GetType() == typeof(PessoaFisica))
                {
                    //verifica se ja está cadastrado o cpf
                    if (_context.DoadorPessoaFisica.Any(q => q.Cpf == doador.CpfCnpj && q.CodDoador!=id))
                    {
                        ModelState.AddModelError("Cpf", "Este CPF já está cadastrado");
                        return new BadRequestObjectResult(ModelState);
                    }

                    PessoaFisica pf = (PessoaFisica)d;
                    pf.Nome = doador.NomeRazaoSocial;
                    pf.Cpf = doador.CpfCnpj;
                    pf.Celular = doador.Celular;
                    pf.Telefone = doador.Telefone;
                    pf.Email = doador.Email;

                    pf.Endereco.Bairro = doador.Endereco.Bairro;
                    pf.Endereco.Cep = doador.Endereco.Cep;
                    pf.Endereco.Cidade = doador.Endereco.Cidade;
                    pf.Endereco.Complemento = doador.Endereco.Complemento;
                    pf.Endereco.Estado = doador.Endereco.Estado;
                    pf.Endereco.Logradouro = doador.Endereco.Logradouro;
                    pf.Endereco.Numero = doador.Endereco.Numero;


                }
                if (d.GetType() == typeof(PessoaJuridica))
                {
                    //verifica se ja está cadastrado o cnpj
                    if (_context.DoadorPessoaJuridica.Any(q => q.Cnpj == doador.CpfCnpj && q.CodDoador != id))
                    {
                        ModelState.AddModelError("Cpf", "Este CPF já está cadastrado");
                        return new BadRequestObjectResult(ModelState);
                    }

                    PessoaJuridica pj = (PessoaJuridica)d;
                    pj.RazaoSocial = doador.NomeRazaoSocial;
                    pj.Cnpj = doador.CpfCnpj;
                    pj.Celular = doador.Celular;
                    pj.Telefone = doador.Telefone;
                    pj.Email = doador.Email;

                    pj.Endereco.Bairro = doador.Endereco.Bairro;
                    pj.Endereco.Cep = doador.Endereco.Cep;
                    pj.Endereco.Cidade = doador.Endereco.Cidade;
                    pj.Endereco.Complemento = doador.Endereco.Complemento;
                    pj.Endereco.Estado = doador.Endereco.Estado;
                    pj.Endereco.Logradouro = doador.Endereco.Logradouro;
                    pj.Endereco.Numero = doador.Endereco.Numero;
                }




                try
                {
                    _context.SaveChanges();
                    return new ObjectResult(doador);
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message.Contains("Email"))
                        ModelState.AddModelError("Email", "Este email ja está cadastrado");

                    return new BadRequestObjectResult(ModelState);
                }
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
            Doador doador = _context.Doador.Include(q=>q.Endereco).Single(q => q.CodDoador == id);
            Endereco endereco = doador.Endereco;
            _context.Doador.Remove(doador);
            _context.Endereco.Remove(endereco);


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
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro, [FromQuery]string tipo)
        {

            //New instance of XlsIO is created.[Equivalent to launching MS Excel with no workbooks open].
            //The instantiation process consists of two steps.

            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            // Creating new workbook
            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet sheet = workbook.Worksheets[0];
            sheet.Name = "Doadores";


            if (String.IsNullOrEmpty(tipo))
                tipo = "PF"; // pessoa fisica

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
            

            sheet.Range[1, 1, 1, 13].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Doadores";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;


            List<DoadorDTO> doadores = new List<DoadorDTO>();
            if (tipo.ToUpper() == "PF")
            {
                IQueryable<PessoaFisica> query = _context.DoadorPessoaFisica.Include(q=>q.Endereco)
                .OrderBy(q => q.Nome);
                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.Nome.Contains(filtro) || q.Cpf.Contains(filtro));


                sheet.Range[3, 1].Text = "Id";
                sheet.Range[3, 2].Text = "Email";
                sheet.Range[3, 3].Text = "Nome";
                sheet.Range[3, 4].Text = "CPF";                
                sheet.Range[3, 5].Text = "Celular";
                sheet.Range[3, 6].Text = "Telefone";
                sheet.Range[3, 7].Text = "Cep";
                sheet.Range[3, 8].Text = "Logradouro";
                sheet.Range[3, 9].Text = "Número";
                sheet.Range[3, 10].Text = "Complemento";
                sheet.Range[3, 11].Text = "Bairro";
                sheet.Range[3, 12].Text = "Cidade";
                sheet.Range[3, 13].Text = "Estado";
                
                IStyle style = sheet[3, 1, 3, 13].CellStyle;
                style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                style.Color = Color.FromArgb(0, 0, 112, 192);
                style.Font.Bold = true;
                style.Font.Color = ExcelKnownColors.White;

                int linha = 4;
                foreach (var v in query)
                {
                    sheet.Range[linha, 1].Number = v.CodDoador;
                    sheet.Range[linha, 2].Text = v.Email;
                    sheet.Range[linha, 3].Text = v.Nome;
                    sheet.Range[linha, 4].Text = v.Cpf.ToCpf();                    
                    sheet.Range[linha, 5].Text = v.Celular.ToTelefone();
                    sheet.Range[linha, 6].Text = v.Telefone.ToTelefone();
                    sheet.Range[linha, 7].Text = v.Endereco.Cep.ToCep();
                    sheet.Range[linha, 8].Text = v.Endereco.Logradouro;
                    sheet.Range[linha, 9].Text = v.Endereco.Numero;
                    sheet.Range[linha, 10].Text = v.Endereco.Complemento;
                    sheet.Range[linha, 11].Text = v.Endereco.Bairro;
                    sheet.Range[linha, 12].Text = v.Endereco.Cidade;
                    sheet.Range[linha, 13].Text = v.Endereco.Estado;                    

                    linha++;
                }

                



            }
            else if (tipo.ToUpper() == "PJ")
            {
                IQueryable<PessoaJuridica> query = _context.DoadorPessoaJuridica.Include(q=>q.Endereco)
               .OrderBy(q => q.RazaoSocial);
                if (!String.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.RazaoSocial.Contains(filtro) || q.Cnpj.Contains(filtro));



                sheet.Range[3, 1].Text = "Id";
                sheet.Range[3, 2].Text = "Email";
                sheet.Range[3, 3].Text = "Razão social";
                sheet.Range[3, 4].Text = "CNPJ";
                sheet.Range[3, 5].Text = "Celular";
                sheet.Range[3, 6].Text = "Telefone";
                sheet.Range[3, 7].Text = "Cep";
                sheet.Range[3, 8].Text = "Logradouro";
                sheet.Range[3, 9].Text = "Número";
                sheet.Range[3, 10].Text = "Complemento";
                sheet.Range[3, 11].Text = "Bairro";
                sheet.Range[3, 12].Text = "Cidade";
                sheet.Range[3, 13].Text = "Estado";

                IStyle style = sheet[3, 1, 3, 13].CellStyle;
                style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                style.Color = Color.FromArgb(0, 0, 112, 192);
                style.Font.Bold = true;
                style.Font.Color = ExcelKnownColors.White;

                int linha = 4;
                foreach (var v in query)
                {
                    sheet.Range[linha, 1].Number = v.CodDoador;
                    sheet.Range[linha, 2].Text = v.Email;
                    sheet.Range[linha, 3].Text = v.RazaoSocial;
                    sheet.Range[linha, 4].Text = v.Cnpj.ToCnpj();
                    sheet.Range[linha, 5].Text = v.Celular.ToTelefone();
                    sheet.Range[linha, 6].Text = v.Telefone.ToTelefone();
                    sheet.Range[linha, 7].Text = v.Endereco.Cep.ToCep();
                    sheet.Range[linha, 8].Text = v.Endereco.Logradouro;
                    sheet.Range[linha, 9].Text = v.Endereco.Numero;
                    sheet.Range[linha, 10].Text = v.Endereco.Complemento;
                    sheet.Range[linha, 11].Text = v.Endereco.Bairro;
                    sheet.Range[linha, 12].Text = v.Endereco.Cidade;
                    sheet.Range[linha, 13].Text = v.Endereco.Estado;

                    linha++;
                }




            }

            #endregion


            if (SaveOption == null)
                SaveOption = "ExcelXlsx";




           

           

           
            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Doador.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Doador.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
