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

        public static List<Dictionary<string, string>> getTickets()
        {
            string jsonOut = request("ticket", new List<string>() {"out=JSONdata"});
            jsonOut = "{ \"Tickets\": " + jsonOut + "}"; 
            AllTickets allTickets =  JsonConvert.DeserializeObject<AllTickets>(jsonOut); 
            
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            
            foreach (_Ticket ticket in allTickets.Tickets)
            {
                Dictionary<string, string> ticketInfo = new Dictionary<string, string>();
                
                ticketInfo["id"] = ticket.id;
                ticketInfo["name"] = ticket.name;
                ticketInfo["client"] = ticket.client;
                ticketInfo["queue"] = ticket.queue;
                ticketInfo["not_blocked"] = ticket.not_blocked;
                ticketInfo["deadline"] = ticket.deadline;
                
                list.Add(ticketInfo);
            }

            return list;
            
            
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