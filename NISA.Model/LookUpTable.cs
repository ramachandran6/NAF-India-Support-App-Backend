using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class LookUpTable
    {
        [Key]
        public int id { get; set; }
        public string? value { get; set; }
        public string? category { get; set; }
    }
}
