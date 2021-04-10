using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ApiServer.Common.Http
{
    /// <summary>
    /// 饿汉式-单例模式
    /// https://www.shuzhiduo.com/A/o75NZVyj5W/
    /// </summary>
    public class HttpClientHelper
    {
        private static readonly object LockObj = new object();
        private static readonly HttpClient client = new HttpClient();
        private static HttpClientHelper clientHelper = null;

        /// <summary>
        /// 构造函数为 private，这样该类就不会被实例化
        /// </summary>
        private HttpClientHelper()
        {
            GetInstance();
        }

        /// <summary>
        /// 获取唯一可用的对象
        /// </summary>
        /// <returns></returns>
        public static HttpClientHelper GetInstance()
        {
            if (clientHelper == null)
            {
                lock (LockObj)
                {
                    if (clientHelper == null) // 双重检查
                    {
                        clientHelper = new HttpClientHelper();
                    }
                }
            }
            return clientHelper;
        }

        /// <summary>
        /// post异步请求方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public async Task<string> PostAsync(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //由HttpClient发出异步Post请求
                HttpResponseMessage res = await client.PostAsync(url, content);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// post同步请求方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public string Post(string url, string strJson)
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //client.DefaultRequestHeaders.Connection.Add("keep-alive");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == HttpStatusCode.OK)
                {
                    string str = res.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string url)
        {
            try
            {
                var responseString = await client.GetStringAsync(url);
                return responseString;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// get同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Get(string url)
        {
            try
            {
                var responseString = client.GetStringAsync(url);
                return responseString.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
