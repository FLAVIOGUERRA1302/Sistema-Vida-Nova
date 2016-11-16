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
using SistemaVidaNova.Services;
using Syncfusion.XlsIO;
using CustomExtensions;
using Syncfusion.Drawing;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize]    
    [Route("api/[controller]")]
    public class DespesaController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEstoqueManager _estoqueManager;

        public DespesaController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            IEstoqueManager estoqueManager)
        {
            _context = context;
            _userManager = userManager;
            _estoqueManager = estoqueManager;


        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<DespesaDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]string tipo)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            if (String.IsNullOrEmpty(tipo))
                tipo = "ASSOCIACAO"; // pessoa fisica

            List<DespesaDTO> despesasDTO = new List<DespesaDTO>();
            switch (tipo)
            {
                case "ASSOCIACAO":
                    IQueryable<DespesaAssociacao> query = _context.DespesaAssociacao
                        .Include(q => q.Item)
                        .Include(q=>q.Usuario)
                        .OrderByDescending(q => q.DataDaCompra);

                    if (!String.IsNullOrEmpty(filtro))
                        query = query.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", query.Count().ToString());
                    var despA = query
                   .Skip(skip.Value)
                   .Take(take.Value).ToList();

                    despesasDTO = despA.Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                        Item = new ItemDTOR()
                        {
                              Id = q.Item.Id,
                               Nome = q.Item.Nome,
                                UnidadeDeMedida = q.Item.UnidadeDeMedida
                        },
                         Usuario = new UsuarioDTOR()
                         {
                             Id = q.Usuario.Id,
                             Nome = q.Usuario.Nome,
                             Email = q.Usuario.Email
                         }
                   }).ToList();

                    break;
                case "FAVORECIDO":
                    IQueryable<DespesaFavorecido> queryF = _context.DespesaFavorecido
                        .Include(q => q.Item)
                        .Include(q=>q.Favorecido)
                        .Include(q => q.Usuario)
                .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryF = queryF.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro) || q.Favorecido.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", queryF.Count().ToString());

                    var despF = queryF
                   .Skip(skip.Value)
                   .Take(take.Value).ToList();

                    despesasDTO = despF.Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                       Item = new ItemDTOR()
                       {
                           Id = q.Item.Id,
                           Nome = q.Item.Nome,
                           UnidadeDeMedida = q.Item.UnidadeDeMedida
                       },
                       Favorecido = new FavorecidoDTOR()
                       {
                            Id = q.Favorecido.CodFavorecido,
                             Apelido = q.Favorecido.Apelido,
                              Cpf = q.Favorecido.Cpf,
                               Nome = q.Favorecido.Nome
                       },
                       Usuario = new UsuarioDTOR()
                       {
                           Id = q.Usuario.Id,
                           Nome = q.Usuario.Nome,
                           Email = q.Usuario.Email
                       }
                   }).ToList();
                    break;
                case "SOPA":
                    IQueryable<DespesaSopa> queryS = _context.DespesaSopa
                        .Include(q => q.Item)
                        .Include(q => q.Usuario)
               .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryS = queryS.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));
                    this.Response.Headers.Add("totalItems", queryS.Count().ToString());

                    var despS = queryS
                   .Skip(skip.Value)
                   .Take(take.Value).ToList();

                    despesasDTO = despS.Select(q => new DespesaDTO
                   {
                       Id = q.Id,
                       Descricao = q.Descricao,
                       DataDaCompra = q.DataDaCompra,
                       Quantidade = q.Quantidade,
                       Tipo = q.Tipo,
                       ValorUnitario = q.ValorUnitario,
                       Item = new ItemDTOR()
                       {
                           Id = q.Item.Id,
                           Nome = q.Item.Nome,
                           UnidadeDeMedida = q.Item.UnidadeDeMedida
                       },
                       Usuario = new UsuarioDTOR()
                       {
                           Id = q.Usuario.Id,
                           Nome = q.Usuario.Nome,
                           Email = q.Usuario.Email
                       }
                   }).ToList();

                    break;
            }






            return despesasDTO;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Despesa despesa =_context.Despesa
                .Include(q=>q.Item)
                .Include(q=>q.Usuario)                
                .SingleOrDefault(q => q.Id == id);

            if (despesa == null)
                return new NotFoundResult();

            DespesaDTO dto = new DespesaDTO();
            if(despesa.GetType() == typeof(DespesaAssociacao))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDTOR()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTOR()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };
            }
            else if (despesa.GetType() == typeof(DespesaSopa))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDTOR()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTOR()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };
            }
            else if (despesa.GetType() == typeof(DespesaFavorecido))
            {
                dto.Id = despesa.Id;
                dto.Descricao = despesa.Descricao;
                dto.DataDaCompra = despesa.DataDaCompra;
                dto.ValorUnitario = despesa.ValorUnitario;
                dto.Quantidade = despesa.Quantidade;
                dto.Tipo = despesa.Tipo;
                dto.Item = new ItemDTOR()
                {
                    Id = despesa.Item.Id,
                    Nome = despesa.Item.Nome,
                    UnidadeDeMedida = despesa.Item.UnidadeDeMedida
                };
                dto.Usuario = new UsuarioDTOR()
                {
                    Id = despesa.Usuario.Id,
                    Nome = despesa.Usuario.Nome,
                    Email = despesa.Usuario.Email
                };

                _context.Favorecido.Where(q => q.CodFavorecido == ((DespesaFavorecido)despesa).CodFavorecido).Load();
                dto.Favorecido = new FavorecidoDTOR()
                {

                    Id = ((DespesaFavorecido)despesa).Favorecido.CodFavorecido,
                    Apelido = ((DespesaFavorecido)despesa).Favorecido.Apelido,
                    Cpf = ((DespesaFavorecido)despesa).Favorecido.Cpf,
                    Nome = ((DespesaFavorecido)despesa).Favorecido.Nome

                };
            }






            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DespesaDTO dto)
        {
            if (ModelState.IsValid && (dto.Tipo.ToUpper() == "ASSOCIACAO" || dto.Tipo.ToUpper() == "FAVORECIDO" || dto.Tipo.ToUpper() == "SOPA"))
            {
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                //corrige fuso horario do js
                dto.DataDaCompra = dto.DataDaCompra.AddHours(-dto.DataDaCompra.Hour);
                switch (dto.Tipo)
                {
                    
                    case "ASSOCIACAO":
                        try
                        {
                            ItemAssociacao ia = _context.ItemAssociacao.Single(q => q.Id == dto.Item.Id);
                            DespesaAssociacao da = new DespesaAssociacao
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = ia,
                                Usuario=user
                            };
                            _context.DespesaAssociacao.Add(da);
                            _context.SaveChanges();
                            dto.Id = da.Id;
                        }
                        catch (Exception e)
                        {
                            //ModelState.AddModelError("Item", "Este item já está cadastrado");
                            return new BadRequestObjectResult(ModelState);
                        }
                        break;
                    case "FAVORECIDO":
                        try
                        {
                            ItemFavorecido ifavorecido = _context.ItemFavorecido.Single(q => q.Id == dto.Item.Id);
                            Favorecido favorecido = _context.Favorecido.Single(q => q.CodFavorecido == dto.Favorecido.Id);
                            DespesaFavorecido df = new DespesaFavorecido
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = ifavorecido,
                                Favorecido = favorecido,
                                 Usuario= user

                            };
                            _context.DespesaFavorecido.Add(df);
                            _context.SaveChanges();
                            dto.Id = df.Id;
                        }
                        catch (Exception e)
                        {
                            ModelState.AddModelError("Favorecido", "Este favorecido não existe mais");
                            return new BadRequestObjectResult(ModelState);
                        }

                        break;
                    case "SOPA":
                        try
                        {
                            ItemSopa isopa = _context.ItemSopa.Single(q => q.Id == dto.Item.Id);
                            DespesaSopa ds = new DespesaSopa
                            {

                                
                                DataDaCompra = dto.DataDaCompra,
                                Descricao = dto.Descricao,
                                Quantidade = dto.Quantidade,
                                ValorUnitario = dto.ValorUnitario,
                                Item = isopa,
                                 Usuario= user
                            };
                            _context.DespesaSopa.Add(ds);
                            _context.SaveChanges();
                            //atualiza o estoque
                            
                            _estoqueManager.DarEntrada(user, isopa, ds.Quantidade);
                            dto.Id = ds.Id;
                        }
                        catch (Exception e)
                        {
                            //ModelState.AddModelError("Item", "Este item já está cadastrado");
                            return new BadRequestObjectResult(ModelState);
                        }
                        break;
                }
                return new ObjectResult(dto);


            }
            else
            {
                if (ModelState.IsValid)
                    ModelState.AddModelError("Tipo", "Valores aceitados = ['ASSOCIACAO','FAVORECIDO','SOPA']");
                return new BadRequestObjectResult(ModelState);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]DespesaDTO dto)
        {
            if (id != dto.Id)
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            if (ModelState.IsValid)
            {
                //corrige fuso horario do js
                dto.DataDaCompra = dto.DataDaCompra.AddHours(-dto.DataDaCompra.Hour);
                Usuario user = await _userManager.GetUserAsync(HttpContext.User);
                Despesa despesa = _context.Despesa.SingleOrDefault(q => q.Id == id);
                if (despesa == null)
                    return new BadRequestResult();
                Item item = _context.Item.SingleOrDefault(q => q.Id == dto.Item.Id);
                if (item == null)
                {
                    ModelState.AddModelError("Item", "Item inválido");
                    return new BadRequestObjectResult(ModelState);
                }
                double diferencaQuantidade = despesa.Quantidade - dto.Quantidade;
                despesa.Item = item;
                despesa.Quantidade = dto.Quantidade;
                despesa.ValorUnitario = dto.ValorUnitario;
                despesa.Usuario = user;


                if (despesa.GetType() == typeof(DespesaAssociacao))
                {
                    DespesaAssociacao desp = (DespesaAssociacao)despesa;
                    //nenhum outro campo para atualizar
                    try
                    {
                        _context.SaveChanges();
                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }
                }
                else if (despesa.GetType() == typeof(DespesaFavorecido))
                {
                    DespesaFavorecido desp = (DespesaFavorecido)despesa;

                    try
                    {
                        Favorecido f = _context.Favorecido.Single(q => q.CodFavorecido == dto.Favorecido.Id);
                        _context.SaveChanges();
                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Favorecido", "Favorecido incorreto");
                        return new BadRequestObjectResult(ModelState);
                    }
                }
                else if (despesa.GetType() == typeof(DespesaSopa))
                {
                    DespesaSopa desp = (DespesaSopa)despesa;
                    //nenhum outro campo para atualizar
                    try
                    {
                        _context.SaveChanges();
                        //atualiza o estoque
                        if (diferencaQuantidade > 0)
                            _estoqueManager.DarSaida(user, item, diferencaQuantidade);
                        else if (diferencaQuantidade < 0)
                            _estoqueManager.DarEntrada(user, item, -diferencaQuantidade);


                        return new ObjectResult(dto);
                    }
                    catch (Exception e)
                    {
                        return new BadRequestObjectResult(ModelState);
                    }
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
            Despesa d = _context.Despesa.Include(q=>q.Item).Single(q => q.Id == id);
            Item item = d.Item;
            double quantidade = d.Quantidade;
            _context.Despesa.Remove(d);

            try
            {
                _context.SaveChanges();
                //atualiza o estoque
                Usuario usuario = await _userManager.GetUserAsync(HttpContext.User);
                _estoqueManager.DarSaida(usuario, item, quantidade);
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
            sheet.Name = "Despesas";


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
            


            sheet.Range[1, 1, 1, 10].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Despesas";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;


            int linha = 1;
            IStyle style;
            switch (tipo)
            {
                case "ASSOCIACAO":
                    IQueryable<DespesaAssociacao> query = _context.DespesaAssociacao
                        .Include(q => q.Item)
                        .Include(q => q.Usuario)
                        .OrderByDescending(q => q.DataDaCompra);

                    if (!String.IsNullOrEmpty(filtro))
                        query = query.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));
                    sheet.Range[3, 1].Text = "Id";
                    sheet.Range[3, 2].Text = "Item";
                    sheet.Range[3, 3].Text = "Descrição";
                    sheet.Range[3, 4].Text = "Quantidade";
                    sheet.Range[3, 5].Text = "Unidade de medida";
                    sheet.Range[3, 6].Text = "Valor unitário";
                    sheet.Range[3, 7].Text = "Valor Total";
                    sheet.Range[3, 8].Text = "Dt. da compra";
                    sheet.Range[3, 9].Text = "Usuário";


                    style = sheet[3, 1, 3, 9].CellStyle;
                    style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    style.Color = Color.FromArgb(0, 0, 112, 192);
                    style.Font.Bold = true;
                    style.Font.Color = ExcelKnownColors.White;

                    linha = 4;
                    foreach (var v in query)
                    {
                        sheet.Range[linha, 1].Number = v.Id;
                        sheet.Range[linha, 2].Text = v.Item.Nome;
                        sheet.Range[linha, 3].Text = v.Descricao;
                        sheet.Range[linha, 4].Number = v.Quantidade;
                        sheet.Range[linha, 5].Text = v.Item.UnidadeDeMedida;
                        sheet.Range[linha, 6].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 6].Number = v.ValorUnitario;
                        sheet.Range[linha, 7].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 7].Formula = "=D" + linha + "*F" + linha;
                        sheet.Range[linha, 8].NumberFormat = "dd/mm/yyyy";
                        sheet.Range[linha, 8].DateTime = v.DataDaCompra;
                        sheet.Range[linha, 9].Text = v.Usuario.Nome;


                        linha++;
                    }

                    break;
                case "FAVORECIDO":
                    IQueryable<DespesaFavorecido> queryF = _context.DespesaFavorecido
                        .Include(q => q.Item)
                        .Include(q => q.Favorecido)
                        .Include(q => q.Usuario)
                .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryF = queryF.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro) || q.Favorecido.Nome.Contains(filtro));
                    sheet.Range[3, 1].Text = "Id";
                    sheet.Range[3, 2].Text = "Item";
                    sheet.Range[3, 3].Text = "Descrição";
                    sheet.Range[3, 4].Text = "Quantidade";
                    sheet.Range[3, 5].Text = "Unidade de medida";
                    sheet.Range[3, 6].Text = "Valor unitário";
                    sheet.Range[3, 7].Text = "Valor Total";
                    sheet.Range[3, 8].Text = "Dt. da compra";
                    sheet.Range[3, 9].Text = "Favorecido";
                    sheet.Range[3, 10].Text = "Usuário";


                    style = sheet[3, 1, 3, 10].CellStyle;
                    style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    style.Color = Color.FromArgb(0, 0, 112, 192);
                    style.Font.Bold = true;
                    style.Font.Color = ExcelKnownColors.White;

                    linha = 4;
                    foreach (var v in queryF)
                    {
                        sheet.Range[linha, 1].Number = v.Id;
                        sheet.Range[linha, 2].Text = v.Item.Nome;
                        sheet.Range[linha, 3].Text = v.Descricao;
                        sheet.Range[linha, 4].Number = v.Quantidade;
                        sheet.Range[linha, 5].Text = v.Item.UnidadeDeMedida;
                        sheet.Range[linha, 6].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 6].Number = v.ValorUnitario;
                        sheet.Range[linha, 7].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 7].Formula = "=D" + linha + "*F" + linha;
                        sheet.Range[linha, 8].NumberFormat = "dd/mm/yyyy";
                        sheet.Range[linha, 8].DateTime = v.DataDaCompra;
                        sheet.Range[linha, 9].Text = v.Favorecido.Nome;
                        sheet.Range[linha, 10].Text = v.Usuario.Nome;


                        linha++;
                    }
                    break;
                case "SOPA":
                    IQueryable<DespesaSopa> queryS = _context.DespesaSopa
                        .Include(q => q.Item)
                        .Include(q=>q.Usuario)
               .OrderByDescending(q => q.DataDaCompra);
                    if (!String.IsNullOrEmpty(filtro))
                        queryS = queryS.Where(q => q.Descricao.Contains(filtro) || q.Item.Nome.Contains(filtro));

                    sheet.Range[3, 1].Text = "Id";
                    sheet.Range[3, 2].Text = "Item";
                    sheet.Range[3, 3].Text = "Descrição";
                    sheet.Range[3, 4].Text = "Quantidade";
                    sheet.Range[3, 5].Text = "Unidade de medida";
                    sheet.Range[3, 6].Text = "Valor unitário";
                    sheet.Range[3, 7].Text = "Valor Total";
                    sheet.Range[3, 8].Text = "Dt. da compra";
                    sheet.Range[3, 9].Text = "Usuário";
                    

                    style = sheet[3, 1, 3, 9].CellStyle;
                    style.VerticalAlignment = ExcelVAlign.VAlignCenter;
                    style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    style.Color = Color.FromArgb(0, 0, 112, 192);
                    style.Font.Bold = true;
                    style.Font.Color = ExcelKnownColors.White;

                    linha = 4;
                    foreach (var v in queryS)
                    {
                        sheet.Range[linha, 1].Number = v.Id;
                        sheet.Range[linha, 2].Text = v.Item.Nome;
                        sheet.Range[linha, 3].Text = v.Descricao;
                        sheet.Range[linha, 4].Number = v.Quantidade;
                        sheet.Range[linha, 5].Text = v.Item.UnidadeDeMedida;
                        sheet.Range[linha, 6].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 6].Number = v.ValorUnitario;
                        sheet.Range[linha, 7].NumberFormat = "R$ #.##00;-R$ #.##00";
                        sheet.Range[linha, 7].Formula = "=D"+linha+"*F"+linha ;                        
                        sheet.Range[linha, 8].NumberFormat = "dd/mm/yyyy";
                        sheet.Range[linha, 8].DateTime = v.DataDaCompra;
                        sheet.Range[linha, 9].Text = v.Usuario.Nome;
                        

                        linha++;
                    }


                    break;
            }

            #endregion


            if (SaveOption == null)
                SaveOption = "ExcelXlsx";









            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Despesa.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Despesa.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }

        [HttpGet("Relatorio")]
        public ActionResult Relatorio([FromQuery]DateTime? start, [FromQuery]DateTime? end, [FromQuery]string tipo)
        {

            if (start == null || end == null || String.IsNullOrEmpty(tipo))
                return new BadRequestResult();

            Dictionary<string, double> resp = new Dictionary<string, double>();

            switch (tipo)
            {
                case "ASSOCIACAO":
                    var query = from d in _context.DespesaAssociacao
                                where d.DataDaCompra >= start.Value && d.DataDaCompra <= end.Value
                                group d by d.Item into g
                                select new
                                {
                                    item = g.Key.Nome,
                                    valor = g.Sum(q => q.ValorUnitario * q.Quantidade)
                                };
                    foreach (var q in query)
                        resp.Add(q.item, q.valor);
                    break;
                case "FAVORECIDO":
                    var queryF = from d in _context.DespesaFavorecido
                                where d.DataDaCompra >= start.Value && d.DataDaCompra <= end.Value
                                group d by d.Item into g
                                select new
                                {
                                    item = g.Key.Nome,
                                    valor = g.Sum(q => q.ValorUnitario * q.Quantidade)
                                };
                    foreach (var q in queryF)
                        resp.Add(q.item, q.valor);
                    break;
                case "SOPA":
                    var queryS = from d in _context.DespesaSopa
                                where d.DataDaCompra >= start.Value && d.DataDaCompra <= end.Value
                                group d by d.Item into g
                                select new
                                {
                                    item = g.Key.Nome,
                                    valor = g.Sum(q => q.ValorUnitario * q.Quantidade)
                                };
                    foreach (var q in queryS)
                        resp.Add(q.item, q.valor);
                    break;
            }

            


            return new ObjectResult(resp);
        }
    }
}
