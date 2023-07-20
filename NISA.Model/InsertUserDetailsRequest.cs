using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class InsertUserDetailsRequest
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? department { get; set; }
        public string? role { get; set; }
        public string? phoneNumber { get; set; }
    }
}
