using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class InsertTicketRequest
    { 
        public string? title { set; get; }
        public string? description { set; get; }
        public string? createdBy { set; get; }
        public string? toDepartment { set; get; }
        public string? startDate { set; get; }
        public string? endDate { set; get; }
        public int? priotity { set; get; }
        public int? severity { set; get; }
        public string? userDepartment { set; get; }
        public string? attachments { set; get;}
    }
}
