using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class ReopenTicketRequest
    {
        public string? title { get; set; }
        public string? description { get; set; }
        public string? endDate { get; set; }
        public int? priority { get; set; }
        public int? severity { get; set; }
        public string? attachments { get; set; }
    }
}
