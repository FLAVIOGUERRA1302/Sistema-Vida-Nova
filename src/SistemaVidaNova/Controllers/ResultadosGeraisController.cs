using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaVidaNova.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;

namespace SistemaVidaNova.Controllers
{
    [Authorize]
    public class ResultadosGeraisController : Controller { 
    
        

        

        public IActionResult Index()
        {
            return View();
        }

       
    }
}
