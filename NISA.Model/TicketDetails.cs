using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace NISA.Model
{
    public class TicketDetails
    {

        [Display(Name = "UserDetails")]
        public virtual int? userId { set; get; }
        [ForeignKey("userId")]
        public virtual UserDetails UserDetails { set; get; }
        public int id { set; get; }
        //public IList<TicketHistoryTable> TicketHistoryTables { get; } = new List<TicketHistoryTable>();
        public string? ticketRefnum { set; get; }
        public string? title { set; get; }
        public string? description { set; get; }
        public string? createdBy { set; get; }
        public string? department { set; get; }
        [Display(Name = "LookUpTable")]
        public virtual int? departmentLookUpId { get; set; }
        [ForeignKey("departmentLookUpId")]
        public virtual LookUpTable LookUpTable { set; get; }
        //public string? toDepartment { set; get; }
        public string? startDate { set; get; }
        public string? endDate { set; get; }
        public string? owner { set; get; }
        public string? status { set; get; }
        public int? priority { set; get; }
        public int? severity { set; get; }
        public int assignedTo { set; get; }
        public int? age { set; get; }
        public string? attachments { set; get; }
        public bool? isDeleted { set; get; }
        public bool? isReopened {  set; get; }

    }
}