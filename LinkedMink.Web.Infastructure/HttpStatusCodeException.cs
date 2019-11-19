using System;

namespace LinkedMink.Web.Infastructure
{
    public class HttpStatusCodeException : ApplicationException
    {
        public HttpStatusCodeException(int errorCode) => ErrorCode = errorCode;

        public int ErrorCode { get; set; }
    }
}
