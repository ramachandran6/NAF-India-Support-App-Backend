using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class CountOfTickets
    {
        public int? assigned { get; set; }
        public int? inProgress { get; set;}
        public int? pendingForUserInfo { get; set; }
        public int? completed { get; set; }
    }
}
