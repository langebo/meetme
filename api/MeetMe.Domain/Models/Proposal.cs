using System;
using System.Collections.Generic;

namespace MeetMe.Domain.Models
{
    public class Proposal
    {
        public Guid Id { get; set; }
        public DateTimeOffset Time { get; set; }
        public List<Vote> Votes { get; set; }

        public Proposal()
        {
            Votes = new List<Vote>();
        }
    }
}