using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
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
        
        private void TicketsViewOnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            ListView view = (ListView) sender; 
            if (e.ColumnIndex != view.Columns.Count - 1)
            {
                if (e.NewWidth <= view.Columns[e.ColumnIndex].Width && view.Columns[e.ColumnIndex].Width > 20)
                {
                    view.Columns[e.ColumnIndex + 1].Width -= e.NewWidth - view.Columns[e.ColumnIndex].Width;
                }
                else
                {
                    if (e.NewWidth >= view.Columns[e.ColumnIndex].Width && view.Columns[e.ColumnIndex + 1].Width > 20)
                    {
                        view.Columns[e.ColumnIndex + 1].Width -= e.NewWidth - view.Columns[e.ColumnIndex].Width;
                    }
                    else
                    {
                        e.NewWidth = view.Columns[e.ColumnIndex].Width;
                        e.Cancel = true;
                    }
                    
                }
            }
        }
        
        public TicketsView()
        {
            Settings = new Settings();
            Settings.LoadColNames();
            
            allTickets = BillmgrHandler.getTickets();
            mainForm = new Form();
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(1000, 600);
            ticketsView = new ListView();
            
            ticketsView.ColumnWidthChanging += TicketsViewOnColumnWidthChanging;
            
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;

            // Загрузка размера колонок из настроек
            Settings.LoadColSizes(mainForm.Size.Width); 
            
            // Загрузка колонок в ListView
            foreach (string col in Settings.ColumnToName.Keys)
            {
                AddColumnToView(Settings.ColumnToName[col], Settings.ColumnToSize[col]);
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