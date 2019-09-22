using System;
using System.Collections.Generic;

namespace MeetMe.Domain.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public List<Vote> Votes { get; set; }

        public Invitation()
        {
            Votes = new List<Vote>();
        }
    }
}