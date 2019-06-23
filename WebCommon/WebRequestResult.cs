using System;
using System.Net.Http;

namespace WebCommon
{
    public class WebRequestResult<T>
    {
        public HttpResponseMessage ServiceResponse { get; set; }
        public bool IsSuccess => ServiceResponse?.StatusCode == System.Net.HttpStatusCode.OK && Exception == null;
        public T Result { get; set; }
        public Exception Exception { get; set; }
    }
}
