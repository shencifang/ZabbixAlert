using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _202012111347
{
    class Request
    {
        public Request(string jsonrpc, string method, int id, string auth, dynamic @params)
        {
            this.jsonrpc = jsonrpc;
            this.method = method;
            this.id = id;
            this.auth = auth;
            this.@params = @params;
        }

        public string jsonrpc { get; set; }
        public string method { get; set; }
        public int id { get; set; }
        public string auth { get; set; }
        public dynamic @params { get; set; }
    }
}
