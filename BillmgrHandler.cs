using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace BILLmanager_app
{   
    public static class BillmgrHandler
    {
        private static string authinfo;

        public static string request(List<string> args)
        {
            authinfo = new StreamReader("LoginInfo.txt").ReadLine(); // В директории с бинарником должен быть этот файл
            return authinfo; 
        }
    }
}