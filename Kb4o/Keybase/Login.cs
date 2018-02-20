using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Scrypt;
using System.Security.Cryptography;

namespace Kb4o.Keybase
{
    public class LoginPassphrase
    {

        public static string ComputePasswordHash(Salt salt, string password)
        {
            string binarySalt = String.Join(
                String.Empty, 
                salt.salt.Select
                (
                    s => Convert.ToString(Convert.ToInt32(s.ToString(), 16), 2).PadLeft(4, '0')
                )
            );

            string phrase = $"{password}{binarySalt}";
            //int iterations = (int)Math.Pow(2, 15);

            //ScryptEncoder encoder = new ScryptEncoder(iterations, 8, 1);
            ScryptEncoder encoder = new ScryptEncoder();
            string hash = encoder.Encode(phrase);

            byte[] v4 = Encoding.UTF8.GetBytes(hash).Skip(192).Take(32).ToArray();
            byte[] v5 = Encoding.UTF8.GetBytes(hash).Skip(224).Take(32).ToArray();

            string pdpka5 = v5.ToString();

            ClientLogin blob = new ClientLogin() {
                ctime = DateTime.Now.ToFileTimeUtc(),
                expire_in = TimeSpan.FromHours(8).Ticks
            };

            blob.body = new Body()
            {
                auth = new Auth()
                {
                    nonce = GenerateNonce(),
                    session = salt.login_session
                },
                key = new Key()
                {
                    kid = "",
                    username = "philwheeler"
                },
                type = "auth",
                version = 1
            };


            return JsonConvert.SerializeObject(blob);
        }

        private static string GenerateNonce()
        {
            string nonce = "";

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[16];
                rng.GetBytes(tokenData);
                nonce = Convert.ToBase64String(tokenData);
            }

            return nonce;
        }

        private string GetKid()
        {
            byte version = byte.Parse("01");
            byte keytype = byte.Parse("20");
            string payload = "DD781230A9BCD459C58E1774C558422A34E296FF";
            byte trailer = byte.Parse("0a");

            return (version + keytype + payload + trailer);
        }

    }

    public class Salt
    {
        public string salt { get; set; }
        public string login_session { get; set; }
        public int pwh_version { get; set; }
        public string uid { get; set; }
        public string csrf_token { get; set; }
    }


    internal class ClientLogin
    {
        public Body body { get; set; }
        public long ctime { get; set; }
        public long expire_in { get; set; }
        public string tag { get { return "signature"; } }
    }

    internal class Body
    {
        public Auth auth { get; set; }
        public Key key { get; set; }
        public string type { get; set; }
        public int version { get; set; }
    }
    internal class Auth
    {
        public string nonce { get; set; }
        public string session { get; set; }
    }

    internal class Key
    {
        public string host { get { return "keybase.io"; } }
        public string kid { get; set; }
        public string username { get; set; }
    }

}
