using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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
        public void UpdateColumnsSize() 
        {
            foreach (ColumnHeader column in ticketsView.Columns)
            {
                column.Width = mainForm.Size.Width * Settings.ColumnToSize[
                    Settings.ColumnToName.FirstOrDefault(x => x.Value == column.Text).Key // Поиск в словаре по тексту колонки
                ] / 100;
            }
        }
        
        private void TicketsViewOnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            void _innerUpdateColSize()
            {
                Settings.ColumnToSize[
                    Settings.ColumnToName.FirstOrDefault(x => x.Value == ticketsView.Columns[e.ColumnIndex].Text).Key
                ] = Convert.ToInt32(
                    (double)e.NewWidth / mainForm.Size.Width * 100); 
            
                Settings.ColumnToSize[
                    Settings.ColumnToName.FirstOrDefault(x => x.Value == ticketsView.Columns[e.ColumnIndex + 1].Text).Key
                ] = Convert.ToInt32(
                    (double)ticketsView.Columns[e.ColumnIndex + 1].Width / mainForm.Size.Width * 100);
            }
            
            ListView view = (ListView) sender; 
            if (e.ColumnIndex != view.Columns.Count - 1)
            {
                Settings.isColSizeDefault = false;
                if (e.NewWidth <= view.Columns[e.ColumnIndex].Width && view.Columns[e.ColumnIndex].Width > 20)
                {
                    view.Columns[e.ColumnIndex + 1].Width -= e.NewWidth - view.Columns[e.ColumnIndex].Width;
                    _innerUpdateColSize();
                }
                else
                {
                    if (e.NewWidth >= view.Columns[e.ColumnIndex].Width && view.Columns[e.ColumnIndex + 1].Width > 20)
                    {
                        view.Columns[e.ColumnIndex + 1].Width -= e.NewWidth - view.Columns[e.ColumnIndex].Width;
                        _innerUpdateColSize();
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
            ticketsView = new ListView();
            
            ticketsView.ColumnWidthChanging += TicketsViewOnColumnWidthChanging;
            
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;

            mainForm = new Form();
            mainForm.Resize += MainFormOnResize;
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(1000, 600);

            ticketsView.Scrollable = false; 
            
            mainForm.Controls.Add(ticketsView);
            // Загрузка размера колонок из настроек
            Settings.LoadColSizes(mainForm.Size.Width); 
            
            // Загрузка колонок в ListView
            foreach (string col in Settings.ColumnToName.Keys)
            {
                AddColumnToView(Settings.ColumnToName[col], Convert.ToInt32(mainForm.Size.Width * Settings.ColumnToSize[col] / 100));
            }

            UpdateColumnsSize();
            
            // Загрузка тикетов в список
            foreach (Dictionary<string,string> ticket in allTickets)
            {
                AddItemToList(new []{ ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"] } );
            }

            
        }

        private void MainFormOnResize(object sender, EventArgs e)
        {
            Settings.LoadColSizes(mainForm.Size.Width);
            UpdateColumnsSize();
        }

        public void Show()
        {
            mainForm.ShowDialog();
        }
    }
}