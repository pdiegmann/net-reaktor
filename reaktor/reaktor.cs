using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace reaktor
{
    public class reaktor
    {
        private Boolean _isLoggedIn = false;
        private Boolean _isLoggingEnabled = false;

        private String _baseUrl;
        public String baseUrl { get { return _baseUrl; } set { _baseUrl = value; } }
        
        private String _mail;
        private String _password;
        private String _token;

        public void login()
        {
            this.login(_mail, _password);
        }

        public void login(String mail, String password)
        {
        }

        public void trigger(String trigger) 
        {
            this.trigger(trigger, null, false);
        }

        public void trigger(String trigger, Dictionary<String, dynamic> parameters)
        {
            this.trigger(trigger, parameters, false);
        }

        public void trigger(String trigger, Boolean saveMode)
        {
            this.trigger(trigger, null, saveMode);
        }

        public void trigger(String trigger, Dictionary<String, dynamic> parameters, Boolean saveMode)
        {
        }
    }
}
