using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BILLmanager_app
{
    public class TicketsView
    {
        public Settings Settings;
        private List<Dictionary<string, string>> allTickets;
        private Form mainForm;
        private ListView ticketsView;
        
        public void AddColumnToView(string text, int size) 
        {
            ticketsView.Columns.Add(text, size);
        }
        public void DeleteColumnFromView(int id) 
        {
            ticketsView.Columns.RemoveAt(id);
        }
        public void AddItemToList(string[] itemParams) 
        {
            ticketsView.Items.Add(new ListViewItem(itemParams));
        }
        public void DeleteItemFromList(ListViewItem item) 
        {
            ticketsView.Items.Remove(item);
        }
        
        public TicketsView()
        {
            Settings = new Settings();
            Settings.ReloadColNames();
            
            allTickets = BillmgrHandler.getTickets();
            mainForm = new Form();
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(1000, 600);
            ticketsView = new ListView();
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;

            // Загрузка колонок в список
            int sizePerField = Convert.ToInt32(mainForm.Size.Width / Settings.ColumnToText.Keys.Count);
            foreach (string field in Settings.ColumnToText.Keys)
            {
                AddColumnToView(field, sizePerField);
            }
            
            // Загрузка тикетов в список
            foreach (Dictionary<string,string> ticket in allTickets)
            {
                AddItemToList(new []{ ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"] } );
            }

            mainForm.Controls.Add(ticketsView);
        }

        public void Show()
        {
            mainForm.ShowDialog();
        }
    }
}