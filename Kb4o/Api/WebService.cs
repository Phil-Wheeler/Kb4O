using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Kb4o.Keybase;
using Newtonsoft.Json;

namespace Kb4o.Api
{
    public class KeybaseWebService
    {
        private readonly string rootUrl = "https://keybase.io/_/api/1.0/";

        public KeybaseWebService()
        {
            Parameters = new Dictionary<string, string>();
            Headers = new Dictionary<string, string>();
        }

        public string Url { get; private set; }
        public string Method { get; private set; }
        public Dictionary<string, string> Parameters { get; set; }
        public Dictionary<string, string> Headers { get; set; }

        public void Login(string User, string Password)
        {
            Salt salt = GetSalt(User);

            Url = $"{rootUrl}login.json";
            Method = "POST";
            Headers.Add("X-CSRF-Token", salt.csrf_token);

            string output = LoginPassphrase.ComputePasswordHash(salt, Password);

            //Parameters.Add("email_or_username", User);
            //Parameters.Add("pdpka5", "");
            //Parameters.Add("pdpka4", "");

            string response = CallService();
        }

        private Salt GetSalt(string user)
        {
            Url = $"{rootUrl}getsalt.json?email_or_username={user}";
            Method = "GET";

            string response = CallService();

            Salt salt = JsonConvert.DeserializeObject<Salt>(response);
            return salt;
        }


        private string CallService()
        {
            string result = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{Url}");
            request.ContentType = "application/json; charset=utf-8";
            request.Accept = "application/json";
            request.Method = Method;
            foreach(KeyValuePair<string, string> entry in Headers)
            {
                request.Headers.Add($"{entry.Key}: {entry.Value}");
            }

            if (Method == "POST")
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(Parameters);
                    writer.Write(json);
                    writer.Flush();
                    writer.Close();
                }
            }

            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
