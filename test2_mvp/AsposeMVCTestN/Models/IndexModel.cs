using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AsposeMVCTestN;

namespace AsposeMVCTestN.Models
{
    public class IndexModelMY: PageModel
    {
        public IFormFile MyFile { set; get; }
        public string MyUrl="";
        public List<string> LangList = new List<string>();
        public Dictionary<string, string> LangDictionary = new Dictionary<string, string>();

        public string TextBefore = "";
        public string TextAfter = "";

        public string SelectedLangFrom { get; set; }
        public string SelectedLangTo { get; set; }
        public IEnumerable<SelectListItem> YaLangs { get; set; }

        public IndexModelMY()
        {
            YaLangs = GetLangs();
            
        }
        public void Fill()
        {
            YaLangs = GetLangs();
        }
        public IEnumerable<SelectListItem> GetLangs()
        {
            if (YaLangs == null)
            {
                YandexTranslator ya = new YandexTranslator();
                var LangList = ya.langsDict();
                var roles = LangList.Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.Key,
                                        Text = x.Key + " - " + x.Value
                                    });
                YaLangs = new SelectList(roles, "Value", "Text");
                return YaLangs;
            }
            return YaLangs;
        }
    }
}
