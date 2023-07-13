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
        [Display(Name="LookUpTable")]
        public virtual int lookupRefId { get; set; }
        [ForeignKey("lookupRefId")]
        public virtual LookUpTable LookUpTables { get; set; }
        public string? phoneNumber { get; set; }


    }
}
