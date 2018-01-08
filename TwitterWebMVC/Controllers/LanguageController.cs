using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterWebMVC.Data;
using TwitterWebMVC.Models;

namespace TwitterWebMVC.Controllers
{
    public class LanguageController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}