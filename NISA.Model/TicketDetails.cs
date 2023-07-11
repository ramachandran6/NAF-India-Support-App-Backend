using System.ComponentModel.DataAnnotations;

namespace NISA.Model
{
    public class TicketDetails
    {
        public int? userId { set; get; }
        [Key]
        public int id { set; get; }
        public string? ticketRefnum { set; get; }
        public string? title { set; get; }
        public string? description { set; get; }
        public string? createdBy { set; get; }
        public string? toDepartment { set; get; }
        public string? startDate { set; get; }
        public string? endDate { set; get; }
        public string? owner { set; get; }
        public string? status { set; get; }
        public int? priotity { set; get; }
        public int? severity { set; get; }
        public string? userDepartment { set; get; }
        public int? age { set; get; }
        public string? attachments { set; get; }
    }
}