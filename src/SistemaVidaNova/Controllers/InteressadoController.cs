using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaVidaNova.Controllers
{
    public class InteressadoController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

      

        [HttpGet]
        public IActionResult Detalhe(string id)
        {
            ViewBag.id = id;
            return View();
        }


    }
}
