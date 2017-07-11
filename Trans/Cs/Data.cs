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

namespace Trans
{
 
    public class Data
    {
        public int code { get; set; }
        public string lang { get; set; }
        public List<string> text { get; set; }
    }

    public class HistoryData
    {
        public string textFrom { get; set; }
        public string textTo { get; set; }
        public string direction { get; set; }
        public int isFaforite { get; set; }
        public string transData { get; set; }
    }
    
}