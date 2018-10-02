using System.Collections.Generic;

namespace BILLmanager_app
{
    public class Settings
    {
        // Переменные для локализации
        public string id_col = "ID",
            name_col = "Name",
            client_col = "Client",
            queue_col = "Queue",
            deadline_col = "Deadline";
        
        public Dictionary<string, string> ColumnToText;

        public Settings()
        {
            ColumnToText = new Dictionary<string, string>();
            ReloadColNames();
        }

        // Заполнение списка соответствий имён колонок с их внутренними именами
        public void ReloadColNames()
        {
            ColumnToText[id_col] = "id";
            ColumnToText[name_col] = "name";
            ColumnToText[client_col] = "client";
            ColumnToText[queue_col] = "queue";
            ColumnToText[deadline_col] = "deadline";
        }
    }
}