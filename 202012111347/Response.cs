using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace _202012111347
{
    class Response
    {
        public Response()
        {
        }

        public string jsonrpc { get; set; }
        public dynamic result = new ExpandoObject();
        public int id { get; set; }
    }
}
