using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace BILLmanager_app
{   
    public static class BillmgrHandler
    {
        private static string authinfo;
        public static string billAddr;

        public static AllTickets getTickets()
        {
            string jsonOut = request("ticket", new List<string>() {"out=JSONdata"});
            jsonOut = "{ \"Tickets\": " + jsonOut + "}"; 
            return JsonConvert.DeserializeObject<AllTickets>(jsonOut); 
        }
        
        public static string request(string func, List<string> listArgs)
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