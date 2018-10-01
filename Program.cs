using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
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
            
            Form mainForm = new Form();
            ListView ticketsView = new ListView();
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;
            
            
            ticketsView.Columns.Add("ID", 100);
            ticketsView.Columns.Add("Name", 300);
            ticketsView.Columns.Add("Client", 300);
            ticketsView.Columns.Add("Queue", 50);
            ticketsView.Columns.Add("Deadline", 50);

            string[] ticketInfo = { allTickets.Tickets[0].id, allTickets.Tickets[0].name, allTickets.Tickets[0].client, allTickets.Tickets[0].queue, allTickets.Tickets[0].deadline }; 
            
            var testItem = new ListViewItem(ticketInfo);
            ticketsView.Items.Add(testItem);
            
            mainForm.Controls.Add(ticketsView);
            mainForm.ShowDialog();
        }

        private static void ButtonHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Kek!");
        }
    }
}