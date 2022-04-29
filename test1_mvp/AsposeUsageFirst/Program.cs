using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;

namespace AsposeUsageFirst
{
    class YandexTranslator
    {
        private Sdk yasdk;
        private string Oauth = "AQAAAAAFqAjTAATuweOIU_3mxERElwAC_g39QCA";
        private string folderID = "b1gjpogclj5t17nhjbd5";
        public YandexTranslator()
        {
            yasdk = new Sdk(new OAuthCredentialsProvider(Oauth));
        }
        public string translate( string langfrom, string langto, params string[] texts)
        {
            //yandex translation example

            var req = new Yandex.Cloud.Ai.Translate.V2.TranslateRequest();
            req.FolderId = folderID;
            req.TargetLanguageCode = langto;
            req.SourceLanguageCode = langfrom;
            foreach (var text in texts)
            {
                req.Texts.Add(text);
            }
            
            var ans = yasdk.Services.Ai.Translate.TranslationService.Translate(req);
            string res = String.Join(" ", ans.Translations);

            return res;
        }
        public List<string> langs()
        {
            var req = new Yandex.Cloud.Ai.Translate.V2.ListLanguagesRequest();
            req.FolderId = folderID;
            var available = yasdk.Services.Ai.Translate.TranslationService.ListLanguages(req);
            List<string> res = new List<string>();
            foreach (var lang in available.Languages)
            {
                res.Add(lang.Code);
            }
            return res;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: add from API documentation of Yandex
            YandexTranslator yandex = new YandexTranslator();

            List<string> langs = yandex.langs();

            Console.WriteLine("Input lang from, available: " + String.Join(" ", langs.ToArray()));
            string langfrom = Console.ReadLine().ToLower();
            Console.WriteLine("Input lang to, available: " + String.Join(" ", langs.ToArray()));
            string langto = Console.ReadLine().ToLower();
            //checking input not done...
            
            //word files hardcoded for mvp
            string pathToFile1 = @"headersENG.docx";

            string pathToFile2 = @"footnotesENG.docx";

            string pathToFile3 = @"textENG.docx";
            //first doc - headers footers
            Aspose.Words.Document doc = new Aspose.Words.Document(pathToFile1);
            var nodes = doc.GetChildNodes(NodeType.HeaderFooter, true);
            string text1 = nodes.First().GetText();
            string text2 = "";
            foreach (var node in nodes)
            {

                Console.WriteLine("found HF = " + node.GetText());
                //text1 = node.GetText();
            }
            //second doc - footnotes
            Aspose.Words.Document doc2 = new Aspose.Words.Document(pathToFile2);
            var nodes2 = doc2.GetChildNodes(NodeType.Footnote, true);
            foreach (var node in nodes2)
            {

                Console.WriteLine("found FOOTNOTE =" + node.GetText());
                text2 = node.GetText();
            }
            //third doc - sections
            //firts paragraph of each(!) section
            Aspose.Words.Document doc3 = new Aspose.Words.Document(pathToFile3);
            var nodes3 = doc3.GetChildNodes(NodeType.Section, true);
            StringBuilder sb3 = new StringBuilder();
            foreach (var node in nodes3)
            {
                var sect = (Section)node;
                var parall = sect.GetChildNodes(NodeType.Paragraph, true);
                if (parall.Any())
                {
                    var parf = parall.First();
                    Console.WriteLine("PARAGRAPH=" + parf.GetText());
                    sb3.Append(parf.GetText());
                }
            }
            
            var ans = yandex.translate(langfrom,langto,text1,text2, sb3.ToString() );

            Console.WriteLine("translation=" + ans);
        }
    }
}
