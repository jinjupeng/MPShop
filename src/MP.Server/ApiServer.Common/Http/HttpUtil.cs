using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApiServer.Common.Http
{
    public class HttpUtil
    {
        //[WebClient, HttpClient, HttpWebRequest ,RestSharp之间的区别与抉择](https://www.cnblogs.com/xiaoliangge/p/9535027.html)
        //[C#中HttpWebRequest、WebClient、HttpClient的使用详解](https://www.jb51.net/article/177025.htm)
        //https://stackoverflow.com/questions/27793761/httpclient-vs-httpwebrequest-for-better-performance-security-and-less-connectio

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string Url, string postDataStr = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/json;charset=UTF-8";

            var response = await request.GetResponseAsync();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
