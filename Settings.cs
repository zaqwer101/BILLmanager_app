using System;
using System.Collections.Generic;

namespace BILLmanager_app
{
    public class Settings 
    {
        // ticket["id"], ticket["name"], ticket["client"], ticket["queue"], ticket["deadline"]
        private List<string> columnsList = new List<string> {"id", "name", "client", "queue", "deadline"};

        public int WinHeigh, WinWidth;
        public Dictionary<string, string> ColumnToName;
        public Dictionary<string, int> ColumnToSize; // Словарь коэффицентов размеров
        public bool IsColSizeDefault; 

        public Settings()
        {
            ColumnToName = new Dictionary<string, string>();
            LoadColNames();
            ColumnToSize = new Dictionary<string, int>();
            IsColSizeDefault = true;
            WinHeigh = 0; WinWidth = 0;
        }

        // Заполнение списка соответствий имён колонок с их внутренними именами
        public void LoadColNames()
        {
            ColumnToName = new Dictionary<string, string>()
            {
                {"id", "ID"},
                {"name", "Name"},
                {"client", "Client"},
                {"queue", "Queue"},
                {"deadline", "Deadline"}
            };
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
    }
}