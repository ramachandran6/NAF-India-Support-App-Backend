using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class TicketHistoryTable
    {
        public int id {  get; set; }
        public string? ticketRefNum { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string? status { get; set; }
        public int? priority { get; set; }
        public int? severity { get; set; }
        public string? department { get; set; }
        [Display(Name ="LookUpTable")]
        public virtual int? departmentLookUpRefId { get; set; }
        [ForeignKey("departmentLookUpRefId")]
        public virtual LookUpTable LookUpTable { get; set; }
        public string? attachments { get; set; }
        public string? endDate { get; set; }
        [Display(Name="UserDetails")]
        public virtual int? updatedBy { get; set; }
        [ForeignKey("updatedBy")]
        public virtual UserDetails UserDetails { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}
