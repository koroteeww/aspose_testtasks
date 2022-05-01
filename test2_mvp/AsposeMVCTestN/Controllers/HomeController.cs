using AsposeMVCTestN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        private static string docurl = "";

        [HttpPost]
        public IActionResult Create(IFormFile MyFile, string MyUrl="")
        {
            if (!string.IsNullOrEmpty(MyUrl))
            {
                if (MyUrl.Contains("google"))
                {
                    //get from google drive without google api
                    //https://drive.google.com/uc?export=download&id=FILE_ID
                    int pos1 = MyUrl.IndexOf("/d/")+3;
                    int pos2 = MyUrl.IndexOf("/", pos1 +1);
                    string fileid = MyUrl.Substring(pos1, pos2 - pos1);
                    string gurl = "https://drive.google.com/uc?export=download&id=" + fileid.Trim();
                    docurl = gurl;
                }
                //get from URL. fill doc memorystream from URL
                docurl = MyUrl;
                //assume we have direct link to docx file on some server
                WebClient client = new WebClient();
                byte[] data = client.DownloadData(docurl);
                doc = new MemoryStream(data);

                return RedirectToAction("Index");
            }
            else if (MyFile == null)
            {
                return View("Error");
            }
            else
            {
                doc = new MemoryStream();
                //copy to stream
                MyFile.CopyTo(doc);
                
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult Translate(string SelectedLangTo, string SelectedLangFrom)
        {
            if (doc == null && string.IsNullOrEmpty(docurl) )
            {
                throw new Exception("doc and url empty :(");
            }
            else
            {
                //Aspose usage
                AsposeWords mvp = new AsposeWords();
                string before = "";
                var textafter = mvp.testTask(doc, out before, SelectedLangFrom, SelectedLangTo);
                var md = new TranslateModel();
                md.textBefore = before;
                md.textAfter = textafter;
                return View(md);
            }
        }

    }
}
