﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NISA.Model;

namespace NISA.DataAccessLayer
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<TicketDetails> ticketDetails { get; set; }
        public DbSet<UserDetails> userDetails { get; set; }
        public DbSet<LookUpTable> lookUpTables { get; set; }
        public DbSet<AttachmentDetails> attachmentDetails { get; set; }
        public DbSet<TicketHistoryTable> ticketHistoryTables { get; set; }
        public DbSet<TicketComments> ticketComments { get; set; }
    }
}