using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class UpdateTicketDetailsRequest
    {
        public string? title { get;set; }
        public string? description { get;set; }
        public string? toDepartment { set; get; }
        public string? endDate { set; get; }
        public int? priority { set; get; }
        public int? severity { set; get; }
        public string? attachments { set; get; }
        public string? status { set; get; }
    }
}
