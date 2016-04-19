using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace LoginSLB.App_Code
{
    public class Login
    {
        private string username = "";
        private string password = "";
        private string hash = "";
        private string salt = "";

        public Login(string un, string pass, string sHash, string sSalt)
        {
            username = un;
            password = pass;
            hash = sHash;
            salt = sSalt;
        }

        public static String HashPassword(string p, string s)
        {
            var combinedPassword = String.Concat(p, s);
            var sha256 = new SHA256Managed();
            var bytes = UTF8Encoding.UTF8.GetBytes(combinedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static String GetRandomSalt(Int32 size)
        {
            var random = new RNGCryptoServiceProvider();
            var salt = new Byte[size];
            random.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public string ValidateLogin()
        {
            string error = "Error.";
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                error = "Voer je gebruikersnaam en wachtwoord in.";
            }
            else if (string.IsNullOrEmpty(username))
            {
                error = "Voer je gebruikersnaam in.";
            }
            else if (string.IsNullOrEmpty(password))
            {
                error = "Voer je wachtwoord in.";
            }
            else
            {
                if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(salt))
                {
                    error = "Gebruikersnaam bestaat niet.";
                }
                else
                {
                    string newHash = HashPassword(password, salt);
                    if (!String.Equals(hash, newHash))
                    {
                        error = "Onjuist wachtwoord.";
                    }
                    else
                    {
                        error = "";
                    }
                }
            }
            return error;
        }

        public string NewSalt()
        {
            string newSalt = "";
            return newSalt = GetRandomSalt(password.Length);
        }

        public string NewHash(string s)
        {
            string newHash = "";
            return newHash = HashPassword(password, s);
        }

        public string GetUsername()
        {
            return username;
        }
    }
}