using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class CustomPasswordHasher
    {
        public static string GetSHA512Password(string password)
        {
            string hash = "";
            SHA512 alg = SHA512.Create();
            byte[] result = alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            hash = Encoding.UTF8.GetString(result);
            return hash;
        }
    }
}
