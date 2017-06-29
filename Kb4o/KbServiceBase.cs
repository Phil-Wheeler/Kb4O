using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kb4o
{
    public class KbServiceBase
    {
        public string Url { get; set; }
        public string Endpoint { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public KbServiceBase()
        {
            Url = "https://keybase.io/_/api/1.0/";
            Params = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }

        public KbServiceBase(string baseUrl, string endpoint)
        {
            Url = baseUrl;
            Endpoint = endpoint;

            Params = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }

        public void AddParameter(string name, string value)
        {
            Params.Add(name, value);
        }

        public void AddHeader(string name, string value)
        {
            Headers.Add(name, value);
        }

        public void Invoke()
        {
            Invoke(Endpoint, true);
        }

        public void Invoke(string endpoint)
        {
            Invoke(endpoint, true);
        }

        private void Invoke(string methodName, bool encode)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{Url}{Endpoint}");
            request.ContentType = "Content-Type: application/json; charset=utf-8";
            request.Accept = "application/json";
            request.Method = Method ?? "GET";

            /*
            string stringData = ""; //place body here
            var data = Encoding.ASCII.GetBytes(stringData); // or UTF8

            request.Method = "PUT";
            request.ContentType = ""; //place MIME type here
            request.ContentLength = data.Length;

            var newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            */


            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string result = reader.ReadToEnd();
            }
        }
    }
}
