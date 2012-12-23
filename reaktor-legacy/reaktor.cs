using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Net;

namespace reaktor
{
    public class reaktor
    {
        private Boolean _isLoggedIn = false;
        private Boolean _isLoggingEnabled = false;

        private String _baseUrl = "http://api.reaktor.io";
        
        private String _mail = String.Empty;
        private String _password = String.Empty;
        private String _token = String.Empty;

        private JSONConverter json = new JSONConverter();

        public Action<Boolean> loginSucceded; // The Boolean is just a dummy as .NET 2.0 does not have parameter-less actions
        public Action<String> loginFailed;
        public Action<Boolean> triggerSucceded; // The Boolean is just a dummy as .NET 2.0 does not have parameter-less actions
        public Action<String> triggerFailed;

        public reaktor()
            : base()
        {
        }

        public reaktor(String mail, String password)
            : this()
        {
            this._mail = mail;
            this._password = password;
            this.login();
        }

        public Boolean login()
        {
            return this.login(_mail, _password);
        }
        public Boolean login(Boolean safeMode)
        {
            return this.login(_mail, _password, safeMode);
        }
        public Boolean login(String mail, String password)
        {
            return this.login(mail, password, true);
        }
        public Boolean login(String mail, String password, Boolean safeMode)
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();

            dict.Add("mail", mail);
            dict.Add("pass", MD5Core.GetHashString(password, Encoding.UTF8).ToLower());

            reaktorRequest req = null; 
            if (safeMode)
                req = new reaktorRequest(_baseUrl + "/login", json.toJSON(dict));
            else
                req = new reaktorRequest(_baseUrl + "/login", json.toJSON(dict), this.loginCallback);
            
            req.run();

            if (safeMode)
            {
                req.asyncState.WaitOne();

                Boolean ok = req.result["ok"] == "true" ? true : false;

                if (!ok)
                    throw new Exception(req.result["reason"]);
                else
                    _token = req.result["token"];

                return ok;
            }

            return true;
        }

        public void loginCallback(Dictionary<String, String> jsonResponse)
        {
            Boolean ok = jsonResponse["ok"] == "true" ? true : false;

            if (!ok)
                loginFailed(jsonResponse["reason"]);
            else
            {
                _token = jsonResponse["token"];
                loginSucceded(true);
            }
        }

        public void trigger(String trigger) 
        {
            this.trigger(trigger, null, false);
        }

        public void trigger(String trigger, Dictionary<String, String> parameters)
        {
            this.trigger(trigger, parameters, false);
        }

        public Boolean trigger(String trigger, Boolean saveMode)
        {
            return this.trigger(trigger, null, saveMode);
        }

        public Boolean trigger(String trigger, Dictionary<String, String> parameters, Boolean safeMode)
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();
            dict.Add("token", _token);
            dict.Add("client", "net");
            dict.Add("safe", safeMode ? "true" : "false");
            dict.Add("name", trigger);
            dict.Add("data", json.toJSON(parameters));

            reaktorRequest req = null;
            if (safeMode)
                req = new reaktorRequest(_baseUrl + "/trigger", json.toJSON(dict));
            else
                req = new reaktorRequest(_baseUrl + "/trigger", json.toJSON(dict), triggerCallback);
            
            req.run();

            if (safeMode)
            {
                req.asyncState.WaitOne();
                Boolean ok = req.result["ok"] == "true" ? true : false;

                if (!ok)
                    throw new Exception(req.result["reason"]);

                return ok;
            }

            return true;
        }

        public void triggerCallback(Dictionary<String, String> jsonResponse)
        {
            Boolean ok = jsonResponse["ok"] == "true" ? true : false;

            if (!ok)
                throw new Exception(jsonResponse["reason"]);
        }
    }
}
