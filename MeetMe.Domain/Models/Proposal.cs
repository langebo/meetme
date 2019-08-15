using System;

namespace MeetMe.Domain.Models
{
    public class Proposal
    {
        public Guid Id { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}