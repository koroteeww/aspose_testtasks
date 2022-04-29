using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;

namespace AsposeMVCTestN
{
    public class AsposeWords
    {
        public string testMVP(Stream document, out string TextBefore, string langfrom = "en", string langto = "ru")
        {
            YandexTranslator yandex = new YandexTranslator();
            //string langfrom = "en";
            //string langto = "ru";
            Document doc = new Document(document);
            var nodes = doc.GetChildNodes(NodeType.HeaderFooter, true);
            string text1 = "";//nodes.First().GetText();
            StringBuilder sb = new StringBuilder();
            
            foreach (var node in nodes)
            {
                sb.Append(node.GetText());
                System.Diagnostics.Debug.Write(node.GetText());
                //Console.WriteLine("found HF = " + node.GetText());
                //text1 = node.GetText();
            }
            
            ////second doc - footnotes
            //Aspose.Words.Document doc2 = new Aspose.Words.Document(pathToFile2);
            //var nodes2 = doc2.GetChildNodes(NodeType.Footnote, true);
            //foreach (var node in nodes2)
            //{

            //    Console.WriteLine("found FOOTNOTE =" + node.GetText());
            //    text2 = node.GetText();
            //}
            ////third doc - sections
            ////firts paragraph of each(!) section
            //Aspose.Words.Document doc3 = new Aspose.Words.Document(pathToFile3);
            var nodes3 = doc.GetChildNodes(NodeType.Section, true);
            StringBuilder sb3 = new StringBuilder();
            foreach (var node in nodes3)
            {
                var sect = (Section)node;
                var parall = sect.GetChildNodes(NodeType.Paragraph, true);
                if (parall.Any())
                {
                    var parf = parall.First();
                    //Console.WriteLine("PARAGRAPH=" + parf.GetText());
                    sb3.Append(parf.GetText());
                }
            }
            text1 = sb.ToString()+" "+sb3.ToString();
            TextBefore = text1;

            var ans = yandex.translate(langfrom, langto, text1);

            return ans;
        }
    }
}
