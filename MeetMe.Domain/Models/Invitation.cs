using System;

namespace MeetMe.Domain.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public User User { get; set; }
    }
}