using System;
using System.Collections.Generic;

namespace BILLmanager_app
{
    [Serializable]
    public class Settings 
    { 
        public Dictionary<string, string> ColumnToName;
        public Dictionary<string, int> ColumnToSize; // Словарь коэффицентов размеров
        public Dictionary<string, int> ColumnToColId; // Словарь соответствия внутреннего имени параметра и положения колонки в таблице 
        public bool IsColSizeDefault; 

        public Settings()
        {
            ColumnToName = new Dictionary<string, string>();
            LoadColNames();
            ColumnToSize = new Dictionary<string, int>();
            IsColSizeDefault = true;
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
                {"deadline", "Deadline"},
                {"not_blocked", "Blocked"}
            };

            // Дефолтное положение для колонок, на данный момент изменять порядок нельзя
            ColumnToColId = new Dictionary<string, int>()
            {
                {"id", 0},
                {"name", 1},
                {"client", 2},
                {"queue", 3},
                {"deadline", 4},
                {"not_blocked", 5}
            };
        }

        // Загрузка коэффицентов размеров колонок, относительных ширине формы
        public void LoadColSizes(int formWidth)
        {
            if (IsColSizeDefault)
            {
                foreach (string col in ColumnToColId.Keys)
                {
                    ColumnToSize[col] = Convert.ToInt32(
                        100 / ColumnToColId.Keys.Count
                        ); 
                }
            }
        }
    }
}