using System.Collections.Generic;

namespace BILLmanager_app
{
    public class Settings
    { 
        public Dictionary<string, string> ColumnToName;
        public Dictionary<string, int> ColumnToSize;

        public Settings()
        {
            ColumnToName = new Dictionary<string, string>();
            LoadColNames();
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

        public void LoadColSizes()
        {
            
        }
    }
}