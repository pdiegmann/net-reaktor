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

            Dictionary<String, String> sub = new Dictionary<String, String>();
            sub.Add("a", "01");
            sub.Add("b", "02");
            sub.Add("c", "03");

            Console.WriteLine(json.toJSON(sub));

            Dictionary<String, String> main = new Dictionary<String, String>();
            main.Add("aa", "1");
            main.Add("bb", "2");
            main.Add("cc", json.toJSON(sub));
            main.Add("dd", "3");
            main.Add("ee", "4");

            string jsonString = json.toJSON(main);
            Console.WriteLine(jsonString);

            Dictionary<String, String> dict = json.fromJSON("{ \"aa\": \"01\", \"bb\": \"02\", \"cc\": \"03\" }");

            foreach (String key in dict.Keys)
            {
                Console.WriteLine(key + ": " + dict[key]);
            }

            Dictionary<String, String> dict2 = new Dictionary<String, String>();

            dict2.Add("mail", "test@test.de");
            dict2.Add("pass", MD5Core.GetHashString("test", Encoding.UTF8).ToLower());
            Console.WriteLine(MD5Core.GetHashString("test", Encoding.UTF8).ToLower());

            Console.WriteLine(json.toJSON(dict2));

            reaktor.reaktor r = new reaktor.reaktor("test@test.de", "test");

            Console.ReadKey();
        }
    }
}
