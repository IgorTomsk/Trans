using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Web;
using Newtonsoft.Json;

namespace Trans
{
    public static class Translate
    {

        public static string TranslateTxt (string fortranslate, string direction)
        {
            string apiKey = "trnsl.1.1.20170710T051829Z.b6dda407442ef3b5.9e39bf5e0d11bc07c7ae2d5598f6f04d9f338bb3";
            string urlTranslate = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=";

            string encodeTxt = HttpUtility.UrlEncode(fortranslate);
            string request = urlTranslate + apiKey + "&text=" + encodeTxt + "&lang=" + direction;

            var translated = WebGet.GetData(request);


            Data objTxt = new Data();
            objTxt = JsonConvert.DeserializeObject<Data>(translated);


            return objTxt.text[0];
        }

      

    }
}