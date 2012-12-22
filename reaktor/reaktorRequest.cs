using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace reaktor
{
    private class reaktorRequest
    {
        protected String _url = String.Empty;
        protected String _content = String.Empty;
        protected String _method = "POST";
        protected HttpWebRequest _request = null;

        public reaktorRequest(String url, String content)
            : base()
        {
            this._url = url;
            this._content = content;
        }

        public reaktorRequest(String url, String content, String method)
            : this(url, content)
        {
            this._method = method;
        }

        public void run()
        {
            _request = WebRequest.Create(_url) as HttpWebRequest;
            _request.Method = _method;
            _request.Accept = "application/json";

            object data = new object();
            IAsyncResult asr = _request.BeginGetResponse(new AsyncCallback(callback), null);
        }

        public void callback(IAsyncResult result) 
        {
            _request.EndGetResponse(result);
        }
    }
}
