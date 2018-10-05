using System;
using System.Collections.Generic;

namespace BILLmanager_app
{
    [Serializable]
    public class Settings 
    { 
        public Dictionary<string, string> ColumnToName;
        public Dictionary<string, int> ColumnToSize; // Словарь коэффицентов размеров
        public bool isColSizeDefault; 

        public Settings()
        {
            ColumnToName = new Dictionary<string, string>();
            LoadColNames();
            ColumnToSize = new Dictionary<string, int>();
            isColSizeDefault = true;
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

        // Загрузка коэффицентов размеров колонок, относительных ширине формы
        public void LoadColSizes(int formWidth)
        {
            if (isColSizeDefault)
            {
                foreach (string col in ColumnToName.Keys)
                {
                    ColumnToSize[col] = Convert.ToInt32(
                        100 / ColumnToName.Keys.Count
                        ); 
                }
            }
        }
    }
}