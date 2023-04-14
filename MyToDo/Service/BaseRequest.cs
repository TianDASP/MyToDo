using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class BaseRequest
    {
        public Method Method { get; set; }
        public string Route { get; set; }
        public string ContentType { get; set; } = "application/json";
        public Dictionary<string,string> QueryDic { get; set; } = new Dictionary<string,string>();
        public object Body { get; set; }
        public string JwtToken { get; set; }
    }
}
