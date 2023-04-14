using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        //例如 http://www.so.com/ 
        protected readonly RestClient client;
        public HttpRestClient(Uri apiUrl)
        {
            client = new RestClient(apiUrl); 
        }

        public async Task<RestResponse> ExecuteAsync(BaseRequest baseRequest)
        { 
            var request = new RestRequest(baseRequest.Route, baseRequest.Method);
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (!string.IsNullOrEmpty(baseRequest.JwtToken))
            {
                request.AddHeader("Authorization", $"Bearer {baseRequest.JwtToken}"); 
            }
            if (baseRequest.QueryDic.Count > 0)
            {
                foreach (var item in baseRequest.QueryDic)
                {
                    request.AddParameter(item.Key, item.Value, ParameterType.QueryString);
                } 
            } 
            if(baseRequest.Body!= null)
            {
                request.AddJsonBody(baseRequest.Body);
            }
            return await client.ExecuteAsync(request);
        }
    }
}
