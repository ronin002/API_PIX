using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Application.Services
{
    public static class HashingService
    {
        public static string GetHash(string original)
        {
            MD5 md5 = MD5.Create();
            var salt = "EyyZnnX19G1gzJqHMjPVZOmW6A6L2fgJ";
            byte[] inputBytes = Encoding.ASCII.GetBytes(original + salt);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
