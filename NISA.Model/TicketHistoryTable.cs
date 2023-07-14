using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    class TicketHistoryTable
    {
        public int id {  get; set; }
        [Display(Name ="TicketDetails")]
        public virtual int? ticketId { get; set; }
        [ForeignKey("ticketId")]
        public virtual TicketDetails TicketDetails { get; }
        public string? status { get; set; }
        public string? updatedBy { get; set; }
        public string? updatedOn { get; set; }
    }
}
