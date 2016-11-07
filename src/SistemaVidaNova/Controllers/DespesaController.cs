using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Controllers
{
    [Authorize]
    public class DespesaController : Controller
    {
        // GET: /<controller>/
        

        public IActionResult Index(int id)
        {
            return View();
        }

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Visualizar(int id)
        {

            return View();
        }

        public IActionResult Editar(int id)
        {

            return View();
        }

        [Route("Despesa/Associacao/{a}/{b}")]
        public IActionResult Associacao(string a, string b)
        {

            return View();
        }

        [Route("Despesa/Sopa/{a}/{b}")]
        public IActionResult Sopa(string a, string b)
        {

            return View();
        }

        [Route("Despesa/Favorecido/{a}/{b}")]
        public IActionResult Favorecido(string a, string b)
        {

            return View();
        }

        [Route("Despesa/RelatorioAssociacao/{a}/{b}")]
        public IActionResult RelatorioAssociacao(string a, string b)
        {

            return View();
        }

        [Route("Despesa/RelatorioSopa/{a}/{b}")]
        public IActionResult RelatorioSopa(string a, string b)
        {

            return View();
        }

        [Route("Despesa/RelatorioFavorecido/{a}/{b}")]
        public IActionResult RelatorioFavorecido(string a, string b)
        {

            return View();
        }
    }
}
