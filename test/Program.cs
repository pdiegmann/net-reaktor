using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using reaktor;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            JSONConverter json = new JSONConverter();

            reaktor.reaktor r = new reaktor.reaktor();
            r.loginFailed = loginFailed;
            r.loginSucceded = loginSuccess;
            r.login("test@test.de", "test");

            Dictionary<String, String> dict = new Dictionary<String, String>();
            dict.Add("name", "Phil Diegmann");
            Console.WriteLine("trigger: " + r.trigger("Test", dict, true));

            Console.ReadKey();
        }

        public static void loginSuccess()
        {
            Console.WriteLine("Login success!");
        }

        public static void loginFailed(String reason)
        {
            Console.WriteLine("Login failed:! " + reason);
        }
    }
}
