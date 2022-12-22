using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GRPT.Model.Common
{
    public class ApiResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public dynamic Data { get; set; }
        public ApiResponseModel(HttpStatusCode code, string message, Exception exception, dynamic data = null)
        {
            StatusCode = code;
            Message = message;
            Exception = exception;
            Data = data;
        }
    }
}
