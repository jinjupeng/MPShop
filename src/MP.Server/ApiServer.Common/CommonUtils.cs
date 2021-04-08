using System;
using System.Text;

namespace ApiServer.Common
{
    public class CommonUtils
    {

        /// <summary>
        /// 生成随机用户名，数字和字母组成
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetStringRandom(int length)
        {
            StringBuilder val = new StringBuilder();
            Random random = new Random();

            //参数length，表示生成几位随机数
            for (int i = 0; i < length; i++)
            {
                string charOrNum = random.Next(2) % 2 == 0 ? "char" : "num";
                //输出字母还是数字
                if ("char" == charOrNum)
                {
                    //输出是大写字母还是小写字母
                    int temp = random.Next(2) % 2 == 0 ? 65 : 97;
                    val.Append((char)(random.Next(26) + temp));
                }
                else
                {
                    val.Append(random.Next(10));
                }
            }
            return val.ToString();
        }

    }
}
