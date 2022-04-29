using System;
using System.Collections.Generic;
using System.Linq;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;

namespace AsposeMVCTestN
{
    public class YandexTranslator
    {
        private Sdk yasdk;
        private string Oauth = "AQAAAAAFqAjTAATuweOIU_3mxERElwAC_g39QCA";
        private string folderID = "b1gjpogclj5t17nhjbd5";
        public YandexTranslator()
        {
            yasdk = new Sdk(new OAuthCredentialsProvider(Oauth));
        }
        public string translate(string langfrom, string langto, params string[] texts)
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
        public Dictionary<string, string> langsDict()
        {
            var req = new Yandex.Cloud.Ai.Translate.V2.ListLanguagesRequest();
            req.FolderId = folderID;
            var available = yasdk.Services.Ai.Translate.TranslationService.ListLanguages(req);
            var res = new Dictionary<string, string>();
            foreach (var lang in available.Languages)
            {
                res.Add(lang.Code, lang.Name);
            }
            res = res.OrderBy(item => item.Key).ToDictionary(item => item.Key, item => item.Value);
            return res;
        }
    }
}
