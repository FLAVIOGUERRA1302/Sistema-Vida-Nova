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

        [HttpGet("excel")]
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro)
        {

            IQueryable<Voluntario> query = _context.Voluntario
               .Where(q => q.IsDeletado == false)
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
            sheet.Range[1, 20].ColumnWidth = 15;

            sheet.Range[1, 1, 1, 20].Merge(true);

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
            sheet.Range[3, 20].Text = "Dt. agendamento curso";

            IStyle style = sheet[3, 1, 3, 20].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var v in query)
            {
                sheet.Range[linha, 1].Number = v.Id;
                sheet.Range[linha, 2].Text = v.Email;
                sheet.Range[linha, 3].Text = v.Nome;

                sheet.Range[linha, 4].Text = v.Cpf.ToCpf();
                sheet.Range[linha, 5].Text = v.Rg;
                sheet.Range[linha, 6].Text = v.Celular.ToTelefone();
                sheet.Range[linha, 7].Text = v.Telefone.ToTelefone();
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
                sheet.Range[linha, 18].DateTime = v.DataDeCadastro;
                sheet.Range[linha, 19].NumberFormat = "dd/mm/yyyy";
                sheet.Range[linha, 19].DateTime = v.DataCurso;
                sheet.Range[linha, 20].NumberFormat = "dd/mm/yyyy";
                if (v.DataAgendamentoCurso != null)
                    sheet.Range[linha, 20].DateTime = v.DataAgendamentoCurso.Value;
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
