using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;

using SistemaVidaNova.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SistemaVidaNova.Services;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.IO;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class ResultadoSopaController : Controller
    {
        // GET: api/values
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEstoqueManager _estoqueManager;
        public ResultadoSopaController(VidaNovaContext context, UserManager<Usuario> userManager, IEstoqueManager estoqueManager)
        {
            _context = context;
            _userManager = userManager;
            _estoqueManager = estoqueManager;
        }

        [HttpGet]
        public IEnumerable<ResultadoSopaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;

            IQueryable<ResultadoSopa> query = _context.ResultadoSopa
                .Include(q=>q.Itens)
                .ThenInclude(q=>q.Item)
                .Include(q=>q.ModeloDeReceita)
                .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q =>  q.Descricao.Contains(filtro) || q.ModeloDeReceita.Nome.Contains(filtro) );

            this.Response.Headers.Add("totalItems", query.Count().ToString());
                        
            var list = query.Skip(skip.Value).Take(take.Value).ToList();

            List<ResultadoSopaDTO> modelos = list.Select(q => new ResultadoSopaDTO()
            {
                 Id = q.Id,
                     ModeloDeReceita = new ModeloDeReceitaDTOR()
                     {
                          Id = q.ModeloDeReceita.Id,
                           Nome = q.ModeloDeReceita.Nome
                     },
                   Descricao = q.Descricao,
                   Itens = q.Itens.Select(i=> new ResultadoSopaItemDTO()
                   {
                        Item = new ItemDTOR()
                        {
                             Id = i.Item.Id,
                              Nome = i.Item.Nome,
                               UnidadeDeMedida = i.Item.UnidadeDeMedida
                        },
                         Quantidade = i.Quantidade
                   }).ToList(),
                    Data = q.Data,
                     LitrosProduzidos = q.LitrosProduzidos
            }).ToList();


            return modelos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ResultadoSopa q = _context.ResultadoSopa
                .Include(r => r.Itens)
                .ThenInclude(r => r.Item)
                .Include(r => r.ModeloDeReceita)
                .SingleOrDefault(r => r.Id == id);
            if (q == null)
                return new NotFoundResult();

            ResultadoSopaDTO dto = new ResultadoSopaDTO()
            {
                Id = q.Id,
                 ModeloDeReceita = new ModeloDeReceitaDTOR()
                 {
                      Id=q.ModeloDeReceita.Id,
                       Nome = q.ModeloDeReceita.Nome
                 },
                Descricao = q.Descricao,
                Itens = q.Itens.Select(i => new ResultadoSopaItemDTO()
                {
                    Item = new ItemDTOR()
                    {
                        Id = i.Item.Id,
                        Nome = i.Item.Nome,
                        UnidadeDeMedida = i.Item.UnidadeDeMedida
                    },
                    Quantidade = i.Quantidade
                }).ToList(),
                 Data=q.Data,
                  LitrosProduzidos =q.LitrosProduzidos
            };
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ResultadoSopaDTO dto)
        {
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.Data = dto.Data.AddHours(-dto.Data.Hour);

                if (dto.Itens.Count == 0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }
                ModeloDeReceita mr = _context.ModeloDeReceita.SingleOrDefault(q => q.Id == dto.ModeloDeReceita.Id);
                if (mr == null)
                {
                    ModelState.AddModelError("ModeloDeReceita", "Modelo de receita inexitente");
                    return new BadRequestObjectResult(ModelState);
                }

                ResultadoSopa resultadoSopa = new ResultadoSopa()
                {
                    ModeloDeReceita = mr,
                    Descricao = dto.Descricao,
                    Data = dto.Data,
                    LitrosProduzidos = dto.LitrosProduzidos,
                    Itens = new List<ResultadoSopaItem>()
                };


                var itensNovos = from i in _context.Item
                                 join d in dto.Itens on i.Id equals d.Item.Id
                                 where i.Destino == "SOPA"
                                 select new
                                 {
                                     item = i,
                                     quantidade = d.Quantidade
                                 };
                if (itensNovos.Count() != dto.Itens.Count)
                {
                    ModelState.AddModelError("Itens", "A lista de itens contém itens inválidos");
                    return new BadRequestObjectResult(ModelState);
                }

                Dictionary<Item, double> dicEstoque = new Dictionary<Item, double>();//para atualizar o estoque
                foreach (var i in itensNovos)
                {
                    resultadoSopa.Itens.Add(new ResultadoSopaItem
                    {
                        Item = i.item,
                        Quantidade = i.quantidade
                    });

                    dicEstoque.Add(i.item, i.quantidade);
                }




                try
                {
                    _context.ResultadoSopa.Add(resultadoSopa);

                    _context.SaveChanges();

                    Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);

                    //atualiza o estoque
                    _estoqueManager.DarSaida(usuario, dicEstoque);

                    dto.Id = resultadoSopa.Id;
                    return new ObjectResult(dto);
                }
                catch
                {

                    return new BadRequestObjectResult(ModelState);
                }



            }

            return new BadRequestObjectResult(ModelState);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ResultadoSopaDTO dto)
        {
            if (id != dto.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.Data = dto.Data.AddHours(-dto.Data.Hour);
                if (dto.Itens.Count == 0)
                {
                    ModelState.AddModelError("Itens", "O modelo precisa ter itens");
                    return new BadRequestObjectResult(ModelState);
                }

                ModeloDeReceita mr = _context.ModeloDeReceita.SingleOrDefault(q => q.Id == dto.ModeloDeReceita.Id);
                if (mr == null)
                {
                    ModelState.AddModelError("ModeloDeReceita", "Modelo de receita inexitente");
                    return new BadRequestObjectResult(ModelState);
                }

                ResultadoSopa resultadoSopa = _context.ResultadoSopa
                    .Include(q => q.Itens)
                    .ThenInclude(q => q.Item)
                    .Include(q => q.ModeloDeReceita)
                    .SingleOrDefault(q => q.Id == id);

                resultadoSopa.ModeloDeReceita = mr;
                resultadoSopa.Descricao = dto.Descricao;
                resultadoSopa.Data = dto.Data;
                resultadoSopa.LitrosProduzidos = dto.LitrosProduzidos;


                var itensNovos = from i in _context.Item
                                 join d in dto.Itens on i.Id equals d.Item.Id
                                 where i.Destino == "SOPA"
                                 select new
                                 {
                                     item = i,
                                     quantidade = d.Quantidade
                                 };
                if (itensNovos.Count() != dto.Itens.Count)
                {
                    ModelState.AddModelError("Itens", "A lista de itens contém itens inválidos");
                    return new BadRequestObjectResult(ModelState);
                }
                List<ResultadoSopaItem> itensCorretos = new List<ResultadoSopaItem>();
                Dictionary<Item, double> dicEstoqueSaida = new Dictionary<Item, double>();//para atualizar o estoque
                Dictionary<Item, double> dicEstoqueEntrada = new Dictionary<Item, double>();//para atualizar o estoque
                foreach (var i in itensNovos)
                {
                    var existente = resultadoSopa.Itens.SingleOrDefault(q => q.IdItem == i.item.Id);
                    if (existente == null)
                    {

                        ResultadoSopaItem novoItem = new ResultadoSopaItem
                        {
                            Item = i.item,
                            Quantidade = i.quantidade
                        };

                        resultadoSopa.Itens.Add(novoItem);
                        itensCorretos.Add(novoItem);
                        //para dar saida no estoque 
                        dicEstoqueSaida.Add(i.item, i.quantidade);
                    }
                    else
                    {
                        //se alterou o quantidade do item necessita alterar o estoque
                        double diferenca = existente.Quantidade - i.quantidade;
                        if (diferenca> 0)
                            dicEstoqueEntrada.Add(existente.Item, diferenca);
                        if (diferenca < 0)
                            dicEstoqueSaida.Add(existente.Item, -diferenca);




                        existente.Quantidade = i.quantidade;
                        itensCorretos.Add(existente);
                    }
                }

                //remove os incorretos
                foreach (var item in resultadoSopa.Itens.Except(itensCorretos).ToArray())
                {
                    //para adiciona novamente no estoque os itens q foram removido do resultado
                    dicEstoqueEntrada.Add(item.Item, item.Quantidade);
                    resultadoSopa.Itens.Remove(item);

                }




                try
                {
                    _context.SaveChanges();

                    Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);

                    //atualiza o estoque
                    _estoqueManager.DarSaida(usuario, dicEstoqueSaida);
                    _estoqueManager.DarEntrada(usuario, dicEstoqueEntrada);
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
        public async Task<IActionResult> Delete(int id)
        {
            ResultadoSopa dd = _context.ResultadoSopa.Include(q=>q.Itens).ThenInclude(q=>q.Item).Single(q => q.Id == id);
            Dictionary<Item, double> dicEstoque = new Dictionary<Item, double>();
            foreach(var i in dd.Itens)
            {
                dicEstoque.Add(i.Item, i.Quantidade);
            }
            _context.ResultadoSopa.Remove(dd);
            try
            {
                _context.SaveChanges();

                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);

                //atualiza o estoque                
                _estoqueManager.DarEntrada(usuario, dicEstoque);

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

            IQueryable<ResultadoSopa> query = _context.ResultadoSopa
                 .Include(q => q.Itens)
                 .ThenInclude(q => q.Item)
                 .Include(q => q.ModeloDeReceita)
                 .OrderByDescending(q => q.Data);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(q => q.Descricao.Contains(filtro) || q.ModeloDeReceita.Nome.Contains(filtro));

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
            sheet.Name = "Resultados";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 15;
            sheet.Range[1, 4].ColumnWidth = 30;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;
            sheet.Range[1, 7].ColumnWidth = 15;
            sheet.Range[1, 8].ColumnWidth = 15;



            sheet.Range[1, 1, 1, 8].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Resultado de Sopa";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Modelo de Receita";
            sheet.Range[3, 3].Text = "Data";
            sheet.Range[3, 4].Text = "Descrição";
            sheet.Range[3, 5].Text = "Item";
            sheet.Range[3, 6].Text = "Quantidade";
            sheet.Range[3, 7].Text = "Unidade de medida";
            sheet.Range[3, 8].Text = "Litros produzidos";


            IStyle style = sheet[3, 1, 3, 8].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            int linha = 4;
            foreach (var m in query)
            {
                if (m.Itens.Count > 0)
                {
                    foreach (var i in m.Itens)
                    {
                        sheet.Range[linha, 1].Number = m.Id;
                        sheet.Range[linha, 2].Text = m.ModeloDeReceita.Nome;
                        sheet.Range[linha, 3].NumberFormat = "dd/mm/yyyy";
                        sheet.Range[linha, 3].DateTime = m.Data;
                        sheet.Range[linha, 4].Text = m.Descricao;
                        sheet.Range[linha, 5].Text = i.Item.Nome;                        
                        sheet.Range[linha, 6].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                        sheet.Range[linha, 6].Number = i.Quantidade;
                        sheet.Range[linha, 7].Text = i.Item.UnidadeDeMedida;
                        sheet.Range[linha, 8].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                        sheet.Range[linha, 8].Number = m.LitrosProduzidos;
                        linha++;
                    }
                }
                else
                {
                    sheet.Range[linha, 1].Number = m.Id;
                    sheet.Range[linha, 2].Text = m.ModeloDeReceita.Nome;
                    sheet.Range[linha, 3].NumberFormat = "dd/mm/yyyy";
                    sheet.Range[linha, 3].DateTime = m.Data;
                    sheet.Range[linha, 4].Text = m.Descricao;                    
                    sheet.Range[linha, 6].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                    sheet.Range[linha, 8].NumberFormat = "#,##0.00_ ;[red]-#,##0.00 ";
                    sheet.Range[linha, 8].Number = m.LitrosProduzidos;
                    linha++;
                    linha++;
                }


            }



            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "ResultadoDeSopa.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "ResultadoDeSopa.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
    }
}
