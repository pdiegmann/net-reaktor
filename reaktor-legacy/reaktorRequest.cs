using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace reaktor
{
    class reaktorRequest
    {
        protected String _url = String.Empty;
        protected String _content = String.Empty;
        protected String _contentType = "application/json";
        protected HttpWebRequest _request = null;
        protected Action<Dictionary<String, String>> _callback;

        public Dictionary<String, String> result { get { return _result; } }
        protected Dictionary<String, String> _result;

        protected ManualResetEvent _asyncState;
        public ManualResetEvent asyncState { get { return _asyncState; } }

        public reaktorRequest(String url, String content, Action<Dictionary<String, String>> callback)
            : base()
        {
            this._url = url;
            this._content = content;
            this._callback = callback;
        }

        public reaktorRequest(String url, String content)
            : this(url, content, null, null)
        {
        }

        public reaktorRequest(String url, String content, String contentType)
            : this(url, content, contentType, null)
        {
        }

        public reaktorRequest(String url, String content, String contentType, Action<Dictionary<String, String>> callback)
            : this(url, content, callback)
        {
            if (!String.IsNullOrEmpty(contentType))
                this._contentType = contentType;
        }

        public void run()
        {
            _asyncState = new ManualResetEvent(false);

            // create a new request
            _request = (HttpWebRequest) WebRequest.Create(_url);
            _request.Method = "POST";
            _request.Accept = "application/json";
            _request.ContentType = _contentType;

            if (_request.Method == "POST")
                // Start the asynchronous operation to get the request-content
                _request.BeginGetRequestStream(new AsyncCallback(getRequestCallback), _request);
            else
                // Start the asynchronous operation to get the response
                _request.BeginGetResponse(new AsyncCallback(getResponseCallback), _request);
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
            postStream.Close();

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
            streamResponse.Close();
            streamRead.Close();

            // Release the HttpWebResponse
            response.Close();

            JSONConverter json = new JSONConverter();

            // interpret JSON
            Dictionary<String, String> dict = json.fromJSON(responseString);
            _result = dict;
            _asyncState.Set();

            if (_callback != null)
            {
                // call callback
                _callback(dict);
            }
        }
    }
}
