using System.ComponentModel.DataAnnotations;

namespace NISA.Model
{
    public class UserDetails
    {
        [Key]
        public int id { get; set; }
        public string? userName { get; set; }    
        public string? name { get; set; }
        public string? email { get; set; }   
        public string? password { get; set; }    
        public string? department { get; set; }  
        public bool? isActive { get; set; }
        public string? phoneNumber { get; set; }

    }
}