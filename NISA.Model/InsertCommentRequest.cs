using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class InsertCommentRequest
    {
        public string? ticketRefnum { get; set; }
        public string? comment { get; set; }
    }
}
