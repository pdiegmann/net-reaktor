using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace reaktor
{
    class reaktorRequest
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
            // create a new request
            _request = (HttpWebRequest) WebRequest.Create(_url);
            _request.Method = _method;
            _request.Accept = "application/json";

            IAsyncResult asr;
            if (_request.Method == "POST")
                // Start the asynchronous operation to get the request-content
                asr = _request.BeginGetRequestStream(new AsyncCallback(getRequestCallback), null);
            else
                // Start the asynchronous operation to get the response
                asr = _request.BeginGetResponse(new AsyncCallback(getResponseCallback), null);
        }

        protected void getRequestCallback(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            // End the action
            Stream postStream = request.EndGetRequestStream(result);

            // Convert the string into a byte array. 
            byte[] data = Encoding.UTF8.GetBytes(_content);

            // Write to the request stream.
            postStream.Write(data, 0, data.Length);
            postStream.Dispose();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(getResponseCallback), request);
        }

        protected void getResponseCallback(IAsyncResult result) 
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;

            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            String responseString = streamRead.ReadToEnd();
            // Close the stream object
            streamResponse.Dispose();
            streamRead.Dispose();

            // Release the HttpWebResponse
            response.Dispose();

            JSONConverter json = new JSONConverter();

            // interpret JSON
            Dictionary<String, String> dict = json.fromJSON(responseString);
        }
    }
}
