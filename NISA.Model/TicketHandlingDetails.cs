using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class TicketHandlingDetails
    {
        public int? genUserId { get; set; }
        public int? deptUserId { get; set; }
        public string? ticketId { get; set; }
        [Key]
        public Guid ticketHandleId { get; set; }
    }
}
