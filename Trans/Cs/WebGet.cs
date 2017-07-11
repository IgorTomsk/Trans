using System;
using System.Net;
using System.IO;

namespace Trans
{
    public static class WebGet
    {

        public static string GetData(string url)
        {
            
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "GET";
         
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);

                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                         
                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
           
        }

    }
}