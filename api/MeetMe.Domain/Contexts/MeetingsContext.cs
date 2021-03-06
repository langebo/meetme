using MeetMe.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetMe.Domain.Contexts
{
    public class MeetingsContext : DbContext
    {
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public MeetingsContext(DbContextOptions<MeetingsContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meeting>()
                .OwnsMany(m => m.Proposals);

            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.Creator)
                .WithMany();

            modelBuilder.Entity<Meeting>()
                .HasMany(m => m.Invitations)
                .WithOne();

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.User)
                .WithMany();

            modelBuilder.Entity<Invitation>()
                .OwnsMany(i => i.Votes);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.OidcIdentifier)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.OidcIdentifier)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
        }
    }
}