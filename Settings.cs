using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BILLmanager_app
{
    [Serializable]
    public class Settings 
    {
        // ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"]
        private List<string> columnsList;

        public int WinHeigh, WinWidth;
        public Dictionary<string, string> ColumnToName;
        public Dictionary<string, int> ColumnToSize; // Словарь коэффицентов размеров
        public bool IsColSizeDefault; 

        public Settings()
        {
            columnsList = new List<string> {"id", "name", "client", "queue", "deadline"};
            ColumnToName = new Dictionary<string, string>();

            ColumnToName = new Dictionary<string, string>()
            {
                {"id", "ID"},
                {"name", "Name"},
                {"client", "Client"},
                {"queue", "Queue"},
                {"deadline", "Deadline"}
            };

            ColumnToSize = new Dictionary<string, int>();
            IsColSizeDefault = true;
            WinHeigh = 0; WinWidth = 0;

        }
        // Получить массив колонок
        public List<string> GetColumnsList()
        {
            return this.columnsList;
        }

        public List<string> GetColumnsValues(Dictionary<string, string> ticket)
        {   
            List<string> values = new List<string>();
            foreach(var node in this.GetColumnsList())
            {
                values.Add(ticket[node]);
            }
            return values;
        }

        // Загрузка коэффицентов размеров колонок, относительных ширине формы
        public void LoadColSizes(int formWidth)
        {
            if (IsColSizeDefault)
            {
                foreach (string col in GetColumnsList())
                {
                    ColumnToSize[col] = Convert.ToInt32(
                        100 / columnsList.Count
                        ); 
                }
            }
        }

        public void SaveSettings()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, this);
                Console.WriteLine("Settings saved");
            }
        }

        public static Settings LoadSettings()
        {
            Settings settings; 
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("settings.dat", FileMode.OpenOrCreate))
                {
                    settings = (Settings) formatter.Deserialize(fs);
                    Console.WriteLine("Загрузил настройки");
                }
            }
            catch (Exception e)
            {
                settings = new Settings();
                Console.WriteLine("Не смог загрузить настройки\n" + e.Message);
            }
            return settings;
        }

        public void AddColumn(string name, string intname)
        {

        }
    }
}