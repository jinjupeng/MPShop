using System.Security.Cryptography;
using System.Text;

namespace ApiServer.Common
{
    public static class PasswordEncoder
    {
        private static readonly string signAlgorithm = "SHA256";


        /// <summary>
        /// 对输入的字符串进行加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="signAlgorithm"></param>
        /// <returns></returns>
        public static string Encode(string input)
        {
            // 对要签名的数据计算哈希 
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create(signAlgorithm);
            byte[] hashbytes = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stb = new StringBuilder();
            foreach (byte b in hashbytes)
            {
                // 以十六进制格式格式化 
                stb.Append(b.ToString("x2"));
            }
            return stb.ToString();
        }

        /// <summary>
        /// 判断密码是否一致
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static bool IsMatch(string oldPassword, string newPassword)
        {
            return string.Equals(oldPassword, Encode(newPassword));
        }
    }
}
