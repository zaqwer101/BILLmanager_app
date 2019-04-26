using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters.Binary;
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
        public void AddItemToList(List<string> itemParams) 
        {
            ticketsView.Items.Add(new ListViewItem(itemParams.ToArray()));
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

        private void LoadTickets()
        {
            allTickets = BillmgrHandler.GetTickets(Settings);
        }
        
        private void UpdateTicketsList()
        {
            var newTickets = BillmgrHandler.GetTickets(Settings);

            foreach (var ticket in allTickets)
            {
                // TODO
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
                Settings.IsColSizeDefault = false;
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
            this.Settings = Settings.LoadSettings();

            LoadTickets();
            
            ticketsView = new ListView();
            ticketsView.ColumnWidthChanging += TicketsViewOnColumnWidthChanging;
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;
            ticketsView.Scrollable = true;
            

            if (Settings.WinHeigh == 0 || Settings.WinWidth == 0)
            {
                Settings.WinWidth = 1000;
                Settings.WinHeigh = 600;
            }

            mainForm = new Form();
            mainForm.Resize += MainFormOnResize;
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(Settings.WinWidth, Settings.WinHeigh);
            //mainForm.Icon = new System.Drawing.Icon(Path.GetFullPath(@"image/icon.ico"));

            
            
            mainForm.Controls.Add(ticketsView);
            // Загрузка размера колонок из настроек
            Settings.LoadColSizes(mainForm.Size.Width); 
            
            // Загрузка колонок в ListView
            foreach (var col in Settings.GetColumnsList())
            {
                AddColumnToView(Settings.ColumnToName[col], Convert.ToInt32(mainForm.Size.Width * Settings.ColumnToSize[col] / 100));
            }

            UpdateColumnsSize();
            
            // Загрузка тикетов в список
            foreach (var ticket in allTickets)
            {
                AddItemToList(Settings.GetColumnsValues(ticket));
            }

            this.mainForm.Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object sender, CancelEventArgs e) 
        {
            this.Settings.SaveSettings();
        }

        private void MainFormOnResize(object sender, EventArgs e)
        {
            Settings.LoadColSizes(mainForm.Size.Width);
            UpdateColumnsSize();
            Settings.WinHeigh = mainForm.Height;
            Settings.WinWidth = mainForm.Width;
        }

        public void Show()
        {
            mainForm.ShowDialog();
        }
    }
}