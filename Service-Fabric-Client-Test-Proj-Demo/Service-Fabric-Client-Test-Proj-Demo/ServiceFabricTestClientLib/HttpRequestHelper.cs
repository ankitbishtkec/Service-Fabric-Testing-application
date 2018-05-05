using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceFabricTestClientLib
{
    public class HttpRequestHelper
    {
        public static HttpWebResponse makePOSTRequest( string url, string requestBody = "")
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {

                streamWriter.Write(requestBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }
        public static HttpWebResponse makeGETRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            return (HttpWebResponse)httpWebRequest.GetResponse();
        }
    }
}
