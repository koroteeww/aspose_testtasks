using AsposeMVCTestN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsposeMVCTestN.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new IndexModelMY();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //static works for saving in controller
        private static MemoryStream doc;

        [HttpPost]
        public IActionResult Create(IFormFile MyFile)
        {
            if (MyFile == null)
            {
                return View("Error NULL");
            }
            else
            {
                doc = new MemoryStream();
                MyFile.CopyTo(doc);
                
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Translate(string SelectedLangTo, string SelectedLangFrom)
        {
            if (doc == null)
            {
                throw new Exception("doc null((");
            }
            else
            {
                //Aspose usage
                AsposeWords mvp = new AsposeWords();
                string before = "";
                var textafter = mvp.testMVP(doc, out before, SelectedLangFrom, SelectedLangTo);
                var md = new TranslateModel();
                md.textBefore = before;
                md.textAfter = textafter;
                return View(md);
            }
        }

    }
}
