using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Domain.Contexts
{
    public class MeetingsContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public MeetingsContext(DbContextOptions<MeetingsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.Proposals)
                .WithOne();

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Creator)
                .WithMany();

            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.Invitations)
                .WithOne();

            modelBuilder.Entity<Proposal>()
                .HasMany(p => p.Votes)
                .WithOne();

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany();

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.User)
                .WithMany();
        }
    }
}