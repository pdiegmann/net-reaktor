using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

namespace reaktor
{
    public class reaktor
    {
        private Boolean _isLoggedIn = false;
        private Boolean _isLoggingEnabled = false;

        private String _baseUrl = String.Empty;
        
        private String _mail = String.Empty;
        private String _password = String.Empty;
        private String _token = String.Empty;

        private JSONConverter json = new JSONConverter();

        public void login()
        {
            this.login(_mail, _password);
        }

        public void login(String mail, String password)
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();

            dict.Add("mail", mail);
            dict.Add("pass", MD5Core.GetHash(password).ToString());

            reaktorRequest req = new reaktorRequest(_baseUrl, json.toJSON(dict));
            req.run();
        }

        public void trigger(String trigger) 
        {
            this.trigger(trigger, null, false);
        }

        public void trigger(String trigger, Dictionary<String, String> parameters)
        {
            this.trigger(trigger, parameters, false);
        }

        public void trigger(String trigger, Boolean saveMode)
        {
            this.trigger(trigger, null, saveMode);
        }

        public void trigger(String trigger, Dictionary<String, String> parameters, Boolean saveMode)
        {
            Dictionary<String, String> dict = new Dictionary<String, String>();
            dict.Add("token", _token);
            dict.Add("save", saveMode ? "true" : "false");
            dict.Add("name", trigger);
            dict.Add("data", json.toJSON(parameters));

            reaktorRequest req = new reaktorRequest(_baseUrl, json.toJSON(dict));
            req.run();
        }
    }
}
