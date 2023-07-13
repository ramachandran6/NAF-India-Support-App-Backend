using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class AttachmentDetails
    {
        [Key] public int id { get; set; }
        public int ticketId { get; set; }
        public string fileName { get; set; }
        public string uploadedDate { get; set; }
        public bool isActive { get; set; }

    }
}
