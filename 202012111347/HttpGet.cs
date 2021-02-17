using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace _202012111347
{
    class HttpGet
    {
        public string Get(string url)
        {
            //创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //GET请求
            request.Method = "GET";
            request.ReadWriteTimeout = 5000;
            request.ContentType = "application/json;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//执行get请求
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            //返回内容JSON
            string retString = myStreamReader.ReadToEnd();
            return retString;
        }

    }
}
