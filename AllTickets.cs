using System.Collections.Generic;

namespace BILLmanager_app
{
    public class AllTickets
    {
        public _Ticket[] Tickets { get; set; }

    }

    public class _Ticket
    {
        public string id { get; set; }
        public string name { get; set; }
        public string client { get; set; }
        public string queue { get; set; }
        public string not_blocked { get; set; }
        public string deadline { get; set; }
    }
}