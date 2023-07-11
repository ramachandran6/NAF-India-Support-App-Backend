using Microsoft.EntityFrameworkCore;
using NISA.Model;

namespace NISA.DataAccessLayer
{
    public class dbContext:DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options) { }
        public DbSet<TicketDetails> ticketDetails { get; set; }
    }
}