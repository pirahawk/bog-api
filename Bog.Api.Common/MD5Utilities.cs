using System;
using System.Security.Cryptography;

namespace Bog.Api.Common
{
    public static class MD5Utilities
    {
        public static string ComputeMD5HashBase54(this byte[] input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            using (MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider())
            {
                var md5Hash = md5CryptoServiceProvider.ComputeHash(input);
                return Convert.ToBase64String(md5Hash);
            }
        }
    }
}