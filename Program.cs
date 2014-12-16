using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.DirectoryServices;
using System.Threading;

namespace ldapload
{
    class Program
    {
        static string ldapurl = ConfigurationManager.AppSettings["ldapurl"];
        static string login = ConfigurationManager.AppSettings["login"];
        static string kode = ConfigurationManager.AppSettings["kode"];
        static int antalrequests = int.Parse(ConfigurationManager.AppSettings["antalrequests"]);
        static double antalsekunder = double.Parse(ConfigurationManager.AppSettings["antalsekunder"]);

        static void Main(string[] args)
        {
            Console.WriteLine("version {0}", typeof(Program).Assembly.GetName().Version);
           
            for(var counter = 0;counter<antalrequests;counter++) {
                testQuery(counter + 1);

                if (counter < antalrequests - 1)
                {
                    Thread.Sleep(Convert.ToInt32(antalsekunder * 1000 / antalrequests));
                }
            }
        }

        static void testQuery(int counter)
        {
            try
            {
                using (var de = new DirectoryEntry(ldapurl, login, kode, AuthenticationTypes.Secure))
                using (var deSearch = new DirectorySearcher(de, string.Format("(&(objectClass=user) (cn={0}))", login)))
                {
                    deSearch.FindOne();
                    
                    //hvis man er nået hertil uden exception er login gået godt.
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Ok. ({0})", counter);
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}
