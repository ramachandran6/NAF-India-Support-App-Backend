using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace NISA.Model
{
    public class fileUpload
    {
        [Key] public int id { set; get; }
        public Blob  data;
    }
}
