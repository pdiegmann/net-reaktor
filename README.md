net-reaktor
===========

.NET-library for reaktor.io.
Contains three projects: main and legacy and a console-application for local testing.

Main project supports:
- .NET Framework (4.0.3+)
- Silverlight (4+)
- Windows Phone (7+)
- .NET for Windows App Store

Legacy-project supports:
- .NET 2.0+

Installation
---------
Just add a reference to the build DLL. As always.

Usage
---------

You can either use the asynchronus or the synchronus ('safe') mode.

If you want to go async, implement the following callbacks:

    public void loginSucceded();
    public void loginFailed();
    public void triggerSucceded();
    public void triggerFailed();

Create your reaktor: 

    reaktor.reaktor r = new reaktor.reaktor();
    r.loginSucceded = loginSucceded;
    r.loginFailed = loginFailed;
    r.triggerSucceded = triggerSucceded;
    r.triggerFailed = triggerFailed;
  
Login (async):

    r.login("mail@host.com", "password");

Trigger a trigger (async): 

    Dictionary<String, String> dict = new Dictionary<String, String>();
    dict.Add("parameter_name", "My Value");
    r.trigger("Test", dict);

ATTENTION: You can only trigger a trigger AFTER you were logged in successfully. We recommend to take care of your status with a local variable or use the synchonous ('safe') mode.

If you want to go synchonous, do as follows:

Create your reaktor (synced login): 

    reaktor.reaktor r = new reaktor.reaktor("mail@host.com", "password");
  
Trigger a trigger (sync): 

    Dictionary<String, String> dict = new Dictionary<String, String>();
    dict.Add("parameter_name", "My Value");
    r.trigger("Test", dict, true);

Remember: Although you decided to do the login synchronous, you could send a trigger asynchronous.
