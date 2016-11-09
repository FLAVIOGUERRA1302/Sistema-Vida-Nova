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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SistemaVidaNova.Models.AccountViewModels;
using System.Security.Claims;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Api
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private VidaNovaContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger _logger;
        private IHostingEnvironment _environment;
        public AccountController(
            VidaNovaContext context,
            UserManager<Usuario> userManager,
            ILoggerFactory loggerFactory,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<UsuarioController>();
            _environment = environment;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<UsuarioDTO> Get([FromQuery]int? skip, [FromQuery]int? take, [FromQuery]string orderBy, [FromQuery]string orderDirection, [FromQuery]string filtro, [FromQuery]bool? isAdmin, [FromQuery]bool? isAtivo)
        {

            if (skip == null)
                skip = 0;
            if (take == null)
                take = 1000;
            
            IQueryable<Usuario> query = _context.Usuario                
                .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(u => u.Nome.Contains(filtro) || u.Email.Contains(filtro));

            IdentityRole roleAdmin = _context.Roles.Single(r => r.Name == "Administrator");

            if (isAdmin != null)
            {
                if(isAdmin.Value)
                    query = query.Where(u => u.Roles.Any(r => r.RoleId == roleAdmin.Id));
                else
                    query = query.Where(u => !u.Roles.Any(r => r.RoleId == roleAdmin.Id));
            }
                

            if (isAtivo != null)
                query = query.Where(u => u.IsAtivo == isAtivo.Value);

            this.Response.Headers.Add("totalItems", query.Count().ToString());


            
            var users = from u in _context.Users
                        select u;

            List<UsuarioDTO> Usuarios = query
                .Skip(skip.Value)
                .Take(take.Value).Select(u => new UsuarioDTO
                {
                    Id = u.Id,
                    Cpf = u.Cpf,
                    Email = u.Email,
                    IsAdmin = u.Roles.Any(r => r.RoleId == roleAdmin.Id),
                    IsAtivo = u.IsAtivo,
                    Nome = u.Nome
                }).ToList();

                       


            return Usuarios;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Usuario u =_context.Usuario      
                .Include(q=>q.Roles)          
                .SingleOrDefault(q => q.Id == id);

            if (u == null)
                return new NotFoundResult();

            //_context.ConhecimentoProficional.Where(q => q.CodUsuario == id).Load().;
            //f.ConhecimentosProfissionais = _context.ConhecimentoProficional.Where(q => q.CodUsuario == f.CodUsuario).ToList();
            IdentityRole roleAdmin = _context.Roles.Single(r => r.Name == "Administrator");

            UsuarioDTO dto = new UsuarioDTO
            {
                Id = u.Id,
                Cpf = u.Cpf,
                Email = u.Email,
                IsAdmin = u.Roles.Any(r => r.RoleId == roleAdmin.Id),
                IsAtivo = u.IsAtivo,
                Nome = u.Nome

            };
           
            this.Response.Headers.Add("totalItems", "1");
            return new ObjectResult(dto);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    Nome = model.Nome,
                    UserName = model.Email,
                    Email = model.Email,
                    Cpf = model.Cpf,
                    IsAtivo = true
                };
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");
                        if (model.IsAdmin)
                            await _userManager.AddToRoleAsync(user, "Administrator");

                        List<Claim> claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name,model.Nome),
                          //  new Claim("CPF", model.Cpf)
                        };
                        await _userManager.AddClaimsAsync(user, claims);
                        try
                        {
                            var path = Path.Combine(_environment.WebRootPath, "images\\users\\");
                            System.IO.File.Copy(path + "default.jpg", path + user.Id + ".jpg", true);
                        }
                        catch { };

                        UsuarioDTO dto = new UsuarioDTO
                        {
                            Id = user.Id,
                            Cpf = user.Cpf,
                            Email = user.Email,
                            IsAdmin = model.IsAdmin,
                            IsAtivo = user.IsAtivo,
                            Nome = user.Nome

                        };


                        return new ObjectResult(dto);
                    }
                    AddErrors(result);
                }
                catch
                {
                    ModelState.AddModelError("Cpf", "Este CPF ja está cadastrado");
                }
            }

            return new BadRequestObjectResult(ModelState);

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UsuarioDTO model)
        {
            if (id != model.Id)
                return new BadRequestResult();
            if (ModelState.IsValid)
            {
                var u = _context.Users.Single(q => q.Id == model.Id);

                u.Nome = model.Nome;
                u.IsAtivo = model.IsAtivo;
                u.Cpf = model.Cpf;
                if (model.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(u, "Administrator");
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(u, "Administrator");
                }
                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("CPF", "Este CPF ja está sendo utilizado");
                }

                return new ObjectResult(model);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }


       

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Usuario usuario = _context.Usuario.Single(q => q.Id == id);

            IdentityResult result = await _userManager.DeleteAsync(usuario);
            if (result.Succeeded)
            {
                return new NoContentResult();
            }
            else
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
        public ActionResult CreateExcel([FromQuery]string SaveOption, [FromQuery]string filtro, [FromQuery]bool? isAdmin, [FromQuery]bool? isAtivo)
        {

            IQueryable<Usuario> query = _context.Usuario.Include(q=>q.Roles)
               .OrderBy(q => q.Nome);

            if (!String.IsNullOrEmpty(filtro))
                query = query.Where(u => u.Nome.Contains(filtro) || u.Email.Contains(filtro));

            IdentityRole roleAdmin = _context.Roles.Single(r => r.Name == "Administrator");

            if (isAdmin != null)
            {
                if (isAdmin.Value)
                    query = query.Where(u => u.Roles.Any(r => r.RoleId == roleAdmin.Id));
                else
                    query = query.Where(u => !u.Roles.Any(r => r.RoleId == roleAdmin.Id));
            }


            if (isAtivo != null)
                query = query.Where(u => u.IsAtivo == isAtivo.Value);

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
            sheet.Name = "Usuários";

            #region Generate Excel
            sheet.Range[1, 1].ColumnWidth = 5;
            sheet.Range[1, 2].ColumnWidth = 30;
            sheet.Range[1, 3].ColumnWidth = 30;
            sheet.Range[1, 4].ColumnWidth = 15;
            sheet.Range[1, 5].ColumnWidth = 15;
            sheet.Range[1, 6].ColumnWidth = 15;


            sheet.Range[1, 1, 1, 6].Merge(true);

            //Inserting sample text into the first cell of the first sheet.
            sheet.Range["A1"].Text = "Usuários";
            sheet.Range["A1"].CellStyle.Font.FontName = "Verdana";
            sheet.Range["A1"].CellStyle.Font.Bold = true;
            sheet.Range["A1"].CellStyle.Font.Size = 28;
            sheet.Range["A1"].CellStyle.Font.RGBColor = Color.FromArgb(0, 0, 112, 192);
            sheet.Range["A1"].HorizontalAlignment = ExcelHAlign.HAlignCenter;

            sheet.Range[3, 1].Text = "Id";
            sheet.Range[3, 2].Text = "Nome";
            sheet.Range[3, 3].Text = "Cpf";
            sheet.Range[3, 4].Text = "Email";
            sheet.Range[3, 5].Text = "Administrador";
            sheet.Range[3, 6].Text = "Ativo";


            IStyle style = sheet[3, 1, 3, 6].CellStyle;
            style.VerticalAlignment = ExcelVAlign.VAlignCenter;
            style.HorizontalAlignment = ExcelHAlign.HAlignCenter;
            style.Color = Color.FromArgb(0, 0, 112, 192);
            style.Font.Bold = true;
            style.Font.Color = ExcelKnownColors.White;

            
            int linha = 4;
            foreach (var q in query)
            {
                sheet.Range[linha, 1].Text = q.Id;
                sheet.Range[linha, 2].Text = q.Nome;
                sheet.Range[linha, 3].Text = q.Cpf.ToCpf();
                sheet.Range[linha, 4].Text = q.Email;
                sheet.Range[linha, 5].Text = q.Roles.Any(r => r.RoleId == roleAdmin.Id) ? "sim" : "não";
                sheet.Range[linha, 6].Text = q.IsAtivo ? "sim" : "não";

                linha++;
            }

            #endregion

            string ContentType = null;
            string fileName = null;
            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Usuario.xls";
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "Application/msexcel";
                fileName = "Usuario.xlsx";
            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                switch (error.Code)
                {
                    case "DuplicateUserName":
                        ModelState.AddModelError("Email", "Este usuário já existe, altere o email do usuário");
                        break;
                    case "PasswordRequiresNonAlphanumeric":
                        ModelState.AddModelError("Password", "A senha requer pelo menos 1 caracter especial (diferente de número e letras)");
                        break;
                    case "PasswordRequiresLower":
                        ModelState.AddModelError("Password", "A senha requer pelo menos 1 letra minúscula");
                        break;
                    case "PasswordRequiresUpper":
                        ModelState.AddModelError("Password", "A senha requer pelo menos 1 letra maiúcula");
                        break;
                    case "PasswordRequiresDigit":
                        ModelState.AddModelError("Password", "A senha requer pelo menos 1 número ('0'-'9')");
                        break;
                    default:
                        ModelState.AddModelError("Cpf", "Este CPF já foi cadastrado");
                        break;
                }

            }
        }
    }
}
