using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Xml.Linq;

namespace BILLmanager_app
{   
    public static class BillmgrHandler
    {
        private static string authinfo;
        public static string billAddr;

        public static List<Dictionary<string, string>> getTickets(Settings settings)
        {
            var list = new List<Dictionary<string, string>>();
            string xmlOut = APIRequest("ticket", new List<string>() {"out=xml"});
            XDocument doc = XDocument.Parse(xmlOut);

            foreach(var node in doc.Root.Elements())
            {
                Dictionary<string, string> ticket = new Dictionary<string, string>();
                foreach(var col in settings.ColumnToColId.Keys)
                {
                    ticket[col] = "";
                }

                if (node.Name != "tparams")
                {
                    foreach (var el in node.Descendants())
                    {
                        ticket[el.Name.ToString()] = el.Value;
                    }
                    list.Add(ticket);
                    Console.WriteLine();
                }
            }

            return list;
        }
        
        public static string APIRequest(string func, List<string> listArgs)
        {
            StreamReader sr = new StreamReader("LoginInfo.txt"); // В директории с бинарником должен быть этот файл
            billAddr = sr.ReadLine();
            authinfo = sr.ReadLine(); 
            
            string args = "?func=" + func + "&authinfo=" + authinfo;
            
            foreach (string arg in listArgs)
            {
                args += "&" + arg; 
            }
            
            WebRequest request = WebRequest.Create(billAddr + args);
            WebResponse response = request.GetResponse();
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}