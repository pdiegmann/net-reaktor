using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace reaktor
{
    public class RequestState
    {
        // This class stores the request state of the request. 
        public WebRequest request;
        public RequestState()
        {
            request = null;
        }
    }
}
