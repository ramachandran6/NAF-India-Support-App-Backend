using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class TicketComments
    {
        public int? id {  get; set; }
        public string? ticketRefnum { get; set; }
        public string? comment { get; set; }
        public string? commentedBy { get; set; }
        public DateTime? commentedOn { get; set; }
    }
}
