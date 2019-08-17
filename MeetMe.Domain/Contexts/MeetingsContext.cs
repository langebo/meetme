using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Domain.Contexts
{
    public class MeetingsContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public MeetingsContext(DbContextOptions<MeetingsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.Proposals)
                .WithOne();
            
            modelBuilder.Entity<Proposal>()
                .HasMany(p => p.Votes)
                .WithOne();
        }
    }
}