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

        private void LoadTickets()
        {
            try
            {
                allTickets = BillmgrHandler.getTickets(Settings);
            }
            catch (Exception e)
            {
                MessageBox.Show("Не удалось загрузить список тикетов");
            }
        }

        private void UpdateTicketsView()
        {
            for (int i = 0; i < allTickets.Count; i++)
            {
                ticketsView.Items[i].SubItems[Settings.ColumnToColId["deadline"]].Text = allTickets[i]["deadline"];
            }
        }
        
        private void UpdateTicketsList()
        {
            try
            {
                List<Dictionary<string, string>> newTickets = BillmgrHandler.getTickets(Settings);
                foreach (Dictionary<string, string> ticket in newTickets.ToList()) // Перебираем новые тикеты, обновляем параметры или добавляем в список
                {
                    bool isTicketFound = false; 
                    foreach (Dictionary<string,string> _ticket in allTickets.ToList())
                    {
                        if (ticket["id"] == _ticket["id"])
                        {
                            isTicketFound = true; // Тикет был и при прошлом запросе
                            break;
                        }
                    }

                    if (isTicketFound) // Если тикет как был так и остался, просто обновим нужные параметры
                    {
                        foreach (Dictionary<string,string> _ticket in allTickets.ToList()) // Перебираем все тикеты в поисках нужного
                        {
                            if (_ticket["id"] == ticket["id"])
                            {
                                _ticket["deadline"] = ticket["deadline"];
                                break;
                            }
                        }
                    }
                    else // Если тикет новый, добавим его в список
                    {
                        AddItemToList(new []{ ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"], ticket["not_blocked"]});
                        allTickets.Add(ticket);
                        isTicketFound = false;
                    }
                }

                foreach (Dictionary<string,string> ticket in allTickets.ToList()) // Перебираем старые тикеты, удаляем из списка неактуальные
                {
                    bool isTicketFound = false;
                    foreach (Dictionary<string,string> newTicket in newTickets.ToList())
                    {
                        if (newTicket["id"] == ticket["id"]) // Тикет есть, не удаляем
                        {
                            isTicketFound = true;
                            break;
                        }
                    }

                    if (!isTicketFound)
                    {
                        ticketsView.Items[allTickets.IndexOf(ticket)].Remove();
                        allTickets.Remove(ticket);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
                {
                    Settings = (Settings) formatter.Deserialize(fs);
                    Console.WriteLine("Загрузил настройки");
                }
            }
            catch (Exception e)
            {
                Settings = new Settings();
                Console.WriteLine("Не смог загрузить настройки\n" + e.Message);
            }
            
            Settings.LoadColNames();

            LoadTickets();

            System.Timers.Timer timer = new System.Timers.Timer(10000);
            timer.Elapsed += (sender, args) => 
            {
                UpdateTicketsList(); 
                UpdateTicketsView();                
            };
            
            timer.Enabled = true;
            timer.AutoReset = true;
            
            ticketsView = new ListView();
            ticketsView.ColumnWidthChanging += TicketsViewOnColumnWidthChanging;
            ticketsView.Dock = DockStyle.Fill;
            ticketsView.View = View.Details;
            ticketsView.FullRowSelect = true;
            ticketsView.Scrollable = false; 
            
            mainForm = new Form();
            mainForm.Resize += MainFormOnResize;
            mainForm.Text = "Кекитница";
            mainForm.Size = new Size(1000, 600);
            mainForm.Icon = new System.Drawing.Icon(Path.GetFullPath(@"image/icon.ico"));

            
            
            mainForm.Controls.Add(ticketsView);
            // Загрузка размера колонок из настроек
            Settings.LoadColSizes(mainForm.Size.Width); 
            
            // Загрузка колонок в ListView
            foreach (var col in Settings.ColumnToName.Keys)
            {
                AddColumnToView(Settings.ColumnToName[col], Convert.ToInt32(mainForm.Size.Width * Settings.ColumnToSize[col] / 100));
            }

            UpdateColumnsSize();
            
            // Загрузка тикетов в список
            foreach (var ticket in allTickets)
            {
                AddItemToList(new []{ ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"] } );
            }

            this.mainForm.Closing += OnWindowClosing;
        }

        private void OnWindowClosing(object sender, CancelEventArgs e) 
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Settings);
                Console.WriteLine("Settings saved");
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