using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace BILLmanager_app
{
    internal class Program
    {
        void button_handler()
        {
            
        }
        
        public static void Main(string[] args)
        {
            AllTickets allTickets = BillmgrHandler.getTickets();
            Console.WriteLine(allTickets.Tickets[allTickets.Tickets.Length - 1].deadline); 
        }

        private static void ButtonHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Kek!");
        }
    }
}