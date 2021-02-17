using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace _202012111347
{
    class HttpPost
    {
        public string Postdata(string strpost, string url)
        {
            //表示空字符串，字段为只读
            string json = string.Empty;

            //创建restful请求
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //请求的方法是post
            request.Method = "post";
            //请求的内容类型
            request.ContentType = "application/json";

            //得到参数
            string data = strpost;
            //将字节序列存储到数组中，编码方式为UTF-8
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
            //将数组的元素总和给请求的标头
            request.ContentLength = byteData.Length;

            //以流的形式附加参数
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            //执行请求，达到json
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                //以流的形式读取，返回的就是字符串的json格式
                StreamReader reader = new StreamReader(response.GetResponseStream());
                json = reader.ReadToEnd();
            }
            return json;
        }

    }
}
