using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace BILLmanager_app
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            AllTickets allTickets = BillmgrHandler.getTickets();
            
            Form mainForm = new Form();
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(1000, 600);
            ListView ticketsView = new ListView();
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;
            
            // Добавление колонок 
            ticketsView.Columns.Add("ID", 100);
            ticketsView.Columns.Add("Name", 300);
            ticketsView.Columns.Add("Client", 300);
            ticketsView.Columns.Add("Queue", 50);
            ticketsView.Columns.Add("Deadline", 200);
            
            // Приимер добавления элемента в список
            // string[] ticketInfo = { allTickets.Tickets[0].id, allTickets.Tickets[0].name, allTickets.Tickets[0].client, allTickets.Tickets[0].queue, allTickets.Tickets[0].deadline }; 
            // var testItem = new ListViewItem(ticketInfo);
            // ticketsView.Items.Add(testItem);

            foreach (Ticket ticket in allTickets.Tickets)
            {
                string[] ticketData = {ticket.id, ticket.name, ticket.client, ticket.queue, ticket.deadline};
                ListViewItem item = new ListViewItem(ticketData);
                ticketsView.Items.Add(item);
            }
            
            mainForm.Controls.Add(ticketsView);
            mainForm.ShowDialog();
        }
    }
}