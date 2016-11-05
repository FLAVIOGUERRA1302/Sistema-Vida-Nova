using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using CustomExtensions;
using Syncfusion.Drawing;
using System.IO;
using SistemaVidaNova.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class DoacaoObjetoController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly IEmailSender _emailSender;
        public DoacaoObjetoController(
            VidaNovaContext context,
            IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));

                query = query.Where(q => q.Descricao.Contains(filtro) || doadores.Contains(q.Doador));

            }

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
            DoacaoObjeto q = _context.DoacaoObjeto
                .Include(d=>d.Doador)
                .Include(d=>d.Endereco)
                .Include(d=>d.Voluntario)
                .SingleOrDefault(i => i.Id == id);
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
                },
                Voluntario = new VoluntarioDTOR()
                {
                     Id = q.Voluntario.Id,
                      Nome = q.Voluntario.Nome,
                       Email = q.Voluntario.Email
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

                Voluntario  voluntario = _context.Voluntario.SingleOrDefault(q => q.Id == dto.Voluntario.Id);
                if (voluntario == null)
                {
                    ModelState.AddModelError("Voluntario", "Voluntario inválido");
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
                    },
                    Voluntario = voluntario
                };
                try
                {
                    _context.DoacaoObjeto.Add(novo);
                
                    _context.SaveChanges();
                    enviarEmail(novo);
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

                Voluntario voluntario = _context.Voluntario.SingleOrDefault(q => q.Id == dto.Voluntario.Id);
                if (voluntario == null)
                {
                    ModelState.AddModelError("Voluntario", "Voluntario inválido");
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

                dd.Voluntario = voluntario;

                try
                {
                    _context.SaveChanges();
                    enviarEmail(dd);
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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<DoacaoObjeto> query = _context.DoacaoObjeto
                .Include(q => q.Doador)
                .Include(q=>q.Endereco)
                .Include(q=>q.Voluntario)
                .OrderByDescending(q => q.DataDaDoacao);

            if (!String.IsNullOrEmpty(filtro))
            {
                var doadores = _context.DoadorPessoaFisica.Where(q => q.Nome.Contains(filtro)).Select(q => (Doador)q)
                    .Concat(_context.DoadorPessoaJuridica.Where(q => q.RazaoSocial.Contains(filtro)).Select(q => (Doador)q));

                query = query.Where(q => q.Descricao.Contains(filtro) || doadores.Contains(q.Doador));

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
            sheet.Name = "Doações";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 15;
            sheet.Range[1, 3].ColumnWidth = 15;
            sheet.Range[1, 4].ColumnWidth = 30;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 30;
            sheet.Range[1, 7].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 15;
            sheet.Range[1, 9].ColumnWidth = 15;
            sheet.Range[1, 10].ColumnWidth = 15;
            sheet.Range[1, 11].ColumnWidth = 15;
            sheet.Range[1, 12].ColumnWidth = 15;
            sheet.Range[1, 13].ColumnWidth = 15;
            sheet.Range[1, 14].ColumnWidth = 15;



            sheet.Range[1, 1, 1, 14].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Doações de objeto";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Data de retirada";
            sheet.Range[3, 3].Text = "Data da doação";
            sheet.Range[3, 4].Text = "Doador";
            sheet.Range[3, 5].Text = "Tipo do doador";
            sheet.Range[3, 6].Text = "Descrição";
            sheet.Range[3, 7].Text = "Cep";
            sheet.Range[3, 8].Text = "Logradouro";
            sheet.Range[3, 9].Text = "Número";
            sheet.Range[3, 10].Text = "Complemento";
            sheet.Range[3, 11].Text = "Bairro";
            sheet.Range[3, 12].Text = "Cidade";
            sheet.Range[3, 13].Text = "Estado";
            sheet.Range[3, 14].Text = "Motorista";





            IStyle style = sheet[3, 1, 3, 14].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var q in query)
            {
                sheet.Range[linha, 1].Number = q.Id;
                sheet.Range[linha, 2].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 2].DateTime = q.DataDeRetirada;
                sheet.Range[linha, 3].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 3].DateTime = q.DataDaDoacao;
                sheet.Range[linha, 4].Text = q.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)q.Doador).Nome : ((PessoaJuridica)q.Doador).RazaoSocial;
                sheet.Range[linha, 5].Text = q.Doador.GetType() == typeof(PessoaFisica) ? "PF" : "PJ";
                sheet.Range[linha, 6].Text = q.Descricao;                
                sheet.Range[linha, 7].Text = q.Endereco.Cep.ToCep();
                sheet.Range[linha, 8].Text = q.Endereco.Logradouro;
                sheet.Range[linha, 9].Text = q.Endereco.Numero;
                sheet.Range[linha, 10].Text = q.Endereco.Complemento;
                sheet.Range[linha, 11].Text = q.Endereco.Bairro;
                sheet.Range[linha, 12].Text = q.Endereco.Cidade;
                sheet.Range[linha, 13].Text = q.Endereco.Estado;
                sheet.Range[linha, 14].Text = q.Voluntario.Nome;

                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Doação.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Doação.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }


        private void enviarEmail(DoacaoObjeto doacao)
        {
            string nomeRazaoSocial = doacao.Doador.GetType() == typeof(PessoaFisica) ? ((PessoaFisica)doacao.Doador).Nome : ((PessoaJuridica)doacao.Doador).RazaoSocial;
            string subject = "Buscar doação no dia " + doacao.DataDeRetirada.ToString("dd-MM-yyyy");
            string message = @"Olá  {0},<br/>" +
                @"Temos uma doação para que você possa buscar para a gente, segue os dados da retirada:
                <ul>
                    <li> Doador: {1} </ li >
                    <li> Descrição: {2} </ li >
                    <li> Data de retirada: {3} </ li >
                    <li> Hora de retirada: {4} </ li >
                    <li> Telefone: {5} </ li >
                    <li> Celular: {6} </ li >
                    <li> Email: {7} </ li >
                    <li> Endereço:  
                        <ul>
                            <li> CEP: {8} </ li >
                            <li> Rua/Avenida: {9} </ li >
                            <li> Número: {10} </ li >
                            <li> Complemento: {11} </ li >                       
                            <li> Bairro: {12} </ li >                       
                            <li> Cidade: {13} </ li >                       
                            <li> Estado: {14} </ li >                       
                            
              
                        </ul>
                    </ li >
              
                </ul>
                 <br/> Muito Obrigado <br/>Associação Beneficente Benedito Pacheco (Reintegra Turma da Sopa),<br/>
";


            _emailSender.SendEmailAsync(doacao.Voluntario.Email, subject,
                   String.Format(message,
                   doacao.Voluntario.Nome,
                   nomeRazaoSocial,
                   doacao.Descricao,
                   doacao.DataDeRetirada.ToString("dd-MM-yyyy"),
                   doacao.DataDeRetirada.ToString("HH:mm"),
                   doacao.Doador.Telefone.ToTelefone(),
                   doacao.Doador.Celular.ToTelefone(),
                   doacao.Doador.Email,
                   doacao.Endereco.Cep,
                   doacao.Endereco.Logradouro,
                   doacao.Endereco.Numero,
                   doacao.Endereco.Complemento,
                   doacao.Endereco.Bairro,
                   doacao.Endereco.Cidade,
                   doacao.Endereco.Estado                   
                   )
                   );

        }
    }
}
