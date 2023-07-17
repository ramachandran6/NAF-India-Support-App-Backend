using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class UserDetails
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? userName { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public bool? isActive { get; set; }
        public bool? isLoggedIn { get; set; }
        public string? department { set; get; }
        [Display(Name = "LookUpTable")]
        public virtual int? departmentLookupRefId { get; set; }
        [ForeignKey("departmentLookupRefId")]
        public virtual LookUpTable? LookUpTables { get; set; }
        public string? phoneNumber { get; set; }
        //public IList<TicketDetails> TicketDetails { get; } = new List<TicketDetails>();
    }
}
