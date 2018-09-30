using System.Collections.Generic;

namespace BILLmanager_app
{
    public class AllTickets
    {
        public Ticket[] Tickets { get; set; }
    }

    public class Ticket
    {
        public string id { get; set; }
        public string name { get; set; }
        public string client { get; set; }
        public string queue { get; set; }
        public string not_blocked { get; set; }
        public string deadline { get; set; }
    }
}