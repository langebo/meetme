using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Domain.Contexts
{
    public class MeetingsContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }

        public MeetingsContext(DbContextOptions<MeetingsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .OwnsMany(
                    m => m.Proposals, 
                    mp => mp.OwnsMany(p => p.Votes));
        }
    }
}