using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class InsertAttachmentRequest
    {

        public int ticketId { get; set; }
        public string fileName { get; set; }
        public string uploadedDate { get; set; }
        public bool isActive { get; set; }
    }
}
