using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISA.Model
{
    public class ImageEntity
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string ContentType { get; set; }

        [Required]
        public byte[] ImageData { get; set; }

        public bool? isActive { get; set; }
        public int? ticketId { get; set; }

        public string? uploadedDate { get; set; }

    }
}
