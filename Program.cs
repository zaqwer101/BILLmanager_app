using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace BILLmanager_app
{
    public class Program
    {
        public static AllTickets allTickets;

        
        
        public static void Main(string[] args)
        {
            TicketsView view = new TicketsView();
            view.Show();
        }
    }
}