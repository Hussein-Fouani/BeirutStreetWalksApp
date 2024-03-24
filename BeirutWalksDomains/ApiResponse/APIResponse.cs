using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeirutWalksDomains.ApiResponse
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public List<string> ErrorMessages{ get; set; }  = new List<string>();
        public HttpStatusCode  StatusCode{ get; set; }
    }
}
